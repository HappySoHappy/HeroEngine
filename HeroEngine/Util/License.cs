using HeroEngine.Framework;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace HeroEngine.Util
{
    //we should do a little trickery with the time expiration to prevent programs like cheat engine interefearing
    //server sends time that is *0.8 of the actual time, and the client when displaying that time does *1.2
    //if we determine that the displayed time *0.8 does not equal correct time, we know someone was messing around and we should send info

    //also implement crc checksum checking on the assemblies
    public class License
    {
        public required string LicenseKey = "";
        public required DateTime ExpiryTime;
        public RestrictionHolder? Restrictions;
        public License()
        {

        }

        public static async Task<(bool Success, License? License, string Error)> LoginAsync(string user, string password)
        {
            try
            {
                var requestData = new Dictionary<string, object>
                    {
                        { "action", "login" },
                        { "user", user },
                        { "password", password },
                        { "hardware", Hardware.ToString() },
                        { "version", $"{Placeholders.Build}.{Placeholders.BuildCommit}" }
                    };

                var discordUser = Discord.GetCurrentUser();
                if (discordUser != null)
                {
                    requestData["discord"] = discordUser.Id;
                }

                using (var client = new HttpClient())
                {
                    string auth = Encryption.BcryptEncrypt(user + password + Hardware.ToString() + requestData["version"]);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", auth);

                    var content = new StringContent(Encryption.AESEncrypt(JsonConvert.SerializeObject(requestData)), Encoding.UTF8, "text/plain");
                    var res = await client.PostAsync("http://localhost:3000/api/v1?", content); //http://51.83.180.216:3000/api/v1?

                    if (res.Content.Headers.ContentLength > 0)
                    {
                        var result = await res.Content.ReadAsStringAsync();
                        dynamic responseData = JsonConvert.DeserializeObject(Encryption.AESDecrypt(result))!;
                        if (res.IsSuccessStatusCode)
                        {

                            var license = new License() { LicenseKey = (string)responseData!.token, ExpiryTime = responseData.expiration };
                            return (true, license, "");
                        }
                        else
                        {
                            return (false, null, (string)responseData!.error);
                        }
                    }
                }
            } catch {
                return (false, null, "connection");
            }

            return (false, null, "invalid");
        }

        public static bool KeepAlive()
        {
            return false;
        }
    }
}
