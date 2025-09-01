using Newtonsoft.Json;

namespace HeroEngine.Framework
{
    public class RestrictionHolder
    {
        [JsonProperty("ACCOUNT_LIMIT")]
        public int ActiveAccountLimit { get; set; } = -1;

        [JsonProperty("CHARACTER_SERVER")]
        public string CharacterServer { get; set; } = "";

        [JsonProperty("CHARACTER_GUILD")]
        public string CharacterGuild { get; set; } = "";
    }
}
