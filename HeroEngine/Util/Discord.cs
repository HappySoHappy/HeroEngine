using Newtonsoft.Json;

namespace HeroEngine.Util
{
    public class Discord
    {
        private static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string ClientDb = Path.Combine(AppDataPath, "discord");
        private static readonly string CanaryDb = Path.Combine(AppDataPath, "discordcanary");
        private static readonly string PtbDb = Path.Combine(AppDataPath, "discordptb");

        /// <summary>
        /// Causes memory leak on each call.
        /// </summary>
        /// <returns></returns>
        public static User? GetCurrentUser()
        {
            foreach (var path in new string[] { ClientDb, CanaryDb, PtbDb })
            {
                string statePath = Path.Combine(path, "Local State");
                if (!File.Exists(statePath)) continue;

                string sentry = Path.Combine(path, "sentry");
                if (Directory.Exists(sentry))
                {
                    string[] scopeFiles = Directory.GetFiles(sentry, "scope_*.json", SearchOption.AllDirectories);

                    foreach (var scopeFile in scopeFiles)
                    {
                        using (var fileStream = new FileStream(scopeFile, FileMode.Open, FileAccess.Read))
                        using (var reader = new StreamReader(fileStream))
                        {
                            string contents = reader.ReadToEndAsync().Result;

                            if (string.IsNullOrEmpty(contents) || contents.Length <= 2) continue;

                            dynamic json = JsonConvert.DeserializeObject<dynamic>(contents)!;

                            string id = (string)json.scope.user.id;
                            string name = (string)json.scope.user.username;
                            return new User() { Id = id, Username = name };
                        }
                    }
                }
            }

            return null;
        }

        public class User
        {
            public string Id { get; set; } = "";
            public string Username { get; set; } = "";

            public override string ToString()
            {
                return Username;
            }
        }
    }
}
