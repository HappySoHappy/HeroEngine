using HeroEngine.Persistance;
using System.Net.Http.Headers;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using HeroEngine.Util;
using System.Collections.Specialized;
using System.IO.Compression;
using System.Net.Sockets;
using System.Diagnostics;

namespace HeroEngine.Framework
{
    public class HeroZero
    {
        public static bool Initialized { get; private set; } = false;
        public static int Version { get; private set; } = -1;
        public static int ExpectedVersion { get; private set; } = 231;
        public static int Build { get; private set; } = -1;
        public static int ExpectedBuild { get; private set; } = 108;
        public static ConstantHolder Constants { get; private set; } = new ConstantHolder();
        public static string Hashsalt { get; private set; } = "";
        public static string DeviceInfo = "{\"language\":\"en\",\"pixelAspectRatio\":1,\"screenDPI\":72,\"screenResolutionX\":1920,\"screenResolutionY\":1080,\"touchscreenType\":0,\"os\":\"HTML5\",\"version\":\"WEB 9,3,4,0\"}"; // was WEB 8,9,7,0
        public static string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_5_7) Gecko/20100101 Firefox/66.0";

        public long TsLastAction = 0;
        public long TsLastSync = 0;
        public long TsLastGuildSync = 0;
        public Stack<string> ActionStack = new Stack<string>();
        public DataHolder Data = new DataHolder();
        public HeroZero()
        {

        }

        public void AddAction(string action)
        {
            if (ActionStack.Count > 10)
                ActionStack.Pop();

            ActionStack.Push(action);
        }

        public dynamic PostActionRequest(Account account, Request.Request request, out string error)
        {
            if (!Initialized) throw new Exception("Uninitialized");

            RequestData data = request.Create();

            var values = new OrderedDictionary() // order of this probably doesnt matter
            {
                { "action", request.Action },
                { "user_id", account.Session?.UserId ?? 0 },
                { "user_session_id", account.Session?.SessionId ?? "0" },
                { "client_version", $"html5_{Version}" },
                { "auth", Hash(request.Action, account.Session?.UserId ?? 0) },
                { "build_number", Build },
                { "rct", 1 }, // Connection type (1 HTTP or 2 SOCKET)
                { "keep_active", "true" },
                { "device_type", "web" } // if rct == 2, device_type = socket
            };

            //Reorder data, allow override of values from data
            foreach (var key in new List<string> { "action", "user_id", "user_session_id", "client_version", "auth", "build_number", "rct", "keep_active", "device_type" })
            {
                if (data.Contains(key))
                {
                    var value = data[key];

                    data.RemoveData(key);
                    if (value != null) data.SetData(key, value);
                }
                else
                {
                    data.SetData(key, values[key]);
                }
            }

            dynamic responseData = Post2Endpoint(data.ToString(), account);
            if (responseData.error != null && ((string)responseData.error).Length > 0)
            {
                error = (string)responseData.error;
                return responseData;
            }

            error = "";
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(responseData.data));
        }


        public dynamic Post2Endpoint(string data, Account account)
        {
            int maxRetries = 10;
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    StringContent postContent = new StringContent(data);
                    postContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    CookieContainer cookieContainer = new CookieContainer();

                    using (HttpClientHandler httpClientHandler = new HttpClientHandler()
                    {
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                    })
                    using (HttpClient client = new HttpClient(httpClientHandler))
                    {
                        client.Timeout = TimeSpan.FromMinutes(15); // allow long waits (in case server gets ddosed or something)

                        client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

                        client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
                        client.DefaultRequestHeaders.Add("Priority", "u=0");
                        client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                        client.DefaultRequestHeaders.Add("TE", "trailers");

                        HttpResponseMessage httpResponse = client.PostAsync($"https://{account.Server}.herozerogame.com/request.php", postContent).Result;
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            TsLastAction = UnixTime.Now();

                            string stringResponse = httpResponse.Content.ReadAsStringAsync().Result;
                            if (string.IsNullOrEmpty(stringResponse)) return new { data = new { }, error = "errServerStatus403" };

                            return JsonConvert.DeserializeObject(stringResponse)!;
                        }

                        return new { data = new { }, error = "errServerStatus" + (int)httpResponse.StatusCode };
                    }
                }
                catch (HttpRequestException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
                {
                    account.Logger.Warn($"Network error (attempt {attempt}/{maxRetries}): {ex.Message}");
                }
                catch (TaskCanceledException)
                {
                    account.Logger.Warn($"Request timed out (attempt {attempt}/{maxRetries}).");
                }
                catch (AggregateException ex)
                {
                    account.Logger.Warn($"Network error aggregate (attempt {attempt}/{maxRetries}): {ex.Message}");
                }
                catch (SocketException ex)
                {
                    account.Logger.Warn($"Network error socket (attempt {attempt}/{maxRetries}): {ex.Message}");
                }
                catch (Exception ex)
                {
                    account.Logger.Warn($"Unexpected error: {ex}");
                    break;
                }

                if (attempt < maxRetries)
                {
                    int delayMilliseconds = Math.Max((int)(Math.Pow(2, attempt) * 1000), 10 * 1000);
                    Thread.Sleep(delayMilliseconds);
                }
            }

            return new { data = new { }, error = "Critical HttpClient Failure!" };
        }

        public void DumpStateToFile(string message)
        {
            var logEntry = new
            {
                Timestamp = DateTime.Now,
                CallerMessage = message,
                Caller = new StackTrace(1, true).GetFrame(0)?.GetMethod()?.DeclaringType?.FullName + "." +
                     new StackTrace(1, true).GetFrame(0)?.GetMethod()?.Name,
                HeroZero = new
                {
                    Initialized,
                    Version,
                    Build,
                    Hashsalt
                },
                ActionStack,
                State = JsonConvert.SerializeObject(Data, Formatting.None)
            };

            string json = JsonConvert.SerializeObject(logEntry, Formatting.Indented);
            AppendLogToFile(json + ",");
        }

        private void AppendLogToFile(string message)
        {
            var logs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logs);

            try
            {
                var fileName = Path.Combine(logs, $"dump.log");

                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception)
            {

            }
        }


        public static bool Initialize() // fetch and parse from game scripts
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })
            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");

                //html:
                /*
                 * appCDNUrl = "https://hz-static-2.akamaized.net/";
			appConfigPlatform = "standalone";
			appConfigLocale = "pl_PL";
			appConfigServerId = "pl34";

			/// parse in flash vars for the current app installation
			var clientVars = {
				// IMPORTANT: Also add to application_template.xml for NK
				applicationTitle: "Hero Zero",
				urlPublic: "https://pl34.herozerogame.com/",
				urlRequestServer: "https://pl34.herozerogame.com/request.php",
				urlSocketServer: "https://eu1a-sock1.herozerogame.com",
				urlCDN: "https://hz-static-2.akamaized.net/",
				userId: "0",
				userSessionId: "0",
				testMode: "false",
				debugRunTests: "false",
				registrationSource: "ref=;subid=;lp=;",
				startupParams: "",
				platform: "standalone",
				ssoInfo: "",
				uniqueId: "pl341732052968",
				server_id: "pl34",
				default_locale: "pl_PL",
				localeVersion: "f2e9e218c519a2fda648d2b76a580382234",
				constantsVersion: "f37a200dcef128bf948fc3ba8bf236752347",
				blockRegistration: "false",
				isFriendbarSupported: "true"
			};
                 */


                HttpResponseMessage httpResponseConstants = client.GetAsync($"https://hz-static-2.akamaized.net/assets/data/constants_json.data").Result;
                if (!httpResponseConstants.IsSuccessStatusCode)
                {
                    return false;
                }

                byte[] constantsZlibCompressed = httpResponseConstants.Content.ReadAsByteArrayAsync().Result;
                using (var memoryStream = new MemoryStream(constantsZlibCompressed))
                {
                    // skip zlib header
                    memoryStream.ReadByte();
                    memoryStream.ReadByte();

                    using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
                    using (var resultStream = new MemoryStream())
                    {
                        deflateStream.CopyTo(resultStream);
                        string constants = Encoding.UTF8.GetString(resultStream.ToArray());

                        Constants = JsonConvert.DeserializeObject<ConstantHolder>(constants)!;
                    }
                }

                HttpResponseMessage httpResponseScript = client.GetAsync($"https://hz-static-2.akamaized.net/assets/html5/HeroZero.min.js").Result;
                if (!httpResponseScript.IsSuccessStatusCode)
                {
                    return false;
                }

                string script = httpResponseScript.Content.ReadAsStringAsync().Result;

                Regex versionRegex = new Regex(@"this\.clientVersion=(\d+)", RegexOptions.Singleline);
                if (int.TryParse(versionRegex.Match(script).Groups[1].Value, out var version))
                {
                    Version = version;
                }

                Regex buildRegex = new Regex(@"this\.buildNumber=(\d+)", RegexOptions.Singleline);
                if (int.TryParse(buildRegex.Match(script).Groups[1].Value, out var build))
                {
                    Build = build;
                }

                Regex hashRegex = new Regex(@"\.md5Hash\(a\s*\+\s*""(.*?)""\s*\+\s*b\)", RegexOptions.Singleline);
                Hashsalt = hashRegex.Match(script).Groups[1].Value;

                FileLogger.Instance.Info($"Version: {Version}, Build: {Build}, Hash: {Hashsalt}");
                Initialized = Version != -1 && Build != -1 && Hashsalt.Length > 0;
                return Initialized;
            }
        }

        public static string Hash(string action, int userId)
        {
            if (!Initialized) throw new Exception("Uninitialized");

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes($"{action}{Hashsalt}{userId}");
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
