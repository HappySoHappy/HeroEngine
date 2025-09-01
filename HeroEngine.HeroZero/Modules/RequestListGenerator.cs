using System.Net.Http.Headers;
using System.Net;
using System.Text.RegularExpressions;
using System.Linq;

namespace HeroEngine.HeroZero.Modules
{
    public static class RequestListGenerator
    {
        private static string? GAME_SCRIPT = null;

        private static void DownloadGameScript()
        {
            using (HttpClientHandler httpClientHandler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })
            using (HttpClient client = new HttpClient(httpClientHandler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:12.0) Gecko/20100101 Firefox/12.0");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");

                HttpResponseMessage httpResponseScript = client.GetAsync($"https://hz-static-2.akamaized.net/assets/html5/HeroZero.min.js").Result;
                if (!httpResponseScript.IsSuccessStatusCode)
                {
                    return;
                }

                GAME_SCRIPT = httpResponseScript.Content.ReadAsStringAsync().Result.Replace("\n", "");
            }
        }

        public static void Run()
        {
            DownloadGameScript();
            if (GAME_SCRIPT == null) return;

            Regex countPattern = new Regex(@"p\.application\.sendActionRequest\(""[^""]+""");
            MatchCollection countMatches = countPattern.Matches(GAME_SCRIPT);
            Console.WriteLine($"Found {countMatches.Count} sendActionRequests calls");

            Regex requestPattern = new Regex(@"p\.application\.sendActionRequest\(""([^""]+)"",\s*((?:[a-zA-Z_]\w*|(?:[a-zA-Z_]\w*\.[a-zA-Z_]\w*\([^)]*\))|\{[^}]*\})\s*),\s*m\(");
            MatchCollection requestMatches = requestPattern.Matches(GAME_SCRIPT);

            foreach (Match match in requestMatches)
            {
                string actionRequest = match.Groups[1].Value;
                string arguments = match.Groups[2].Value;

                Console.WriteLine($"{actionRequest} {arguments}");
                Console.WriteLine();
            }

            Console.WriteLine($"Listed {requestMatches.Count} requests");


            Console.WriteLine("Requests not caught by requestPattern:");
            foreach (Match match in countMatches)
            {
                string fullRequest = match.Value;

                bool isCaught = false;
                foreach (Match countMatch in requestMatches)
                {
                    // Check if the start of the match in countMatches is the same as the fullRequest
                    if (countMatch.Value.StartsWith(fullRequest))
                    {
                        isCaught = true;
                        break;
                    }
                }

                if (!isCaught) // Only print if not caught
                {
                    Console.WriteLine(fullRequest);
                }
            }
        }
    }
}
