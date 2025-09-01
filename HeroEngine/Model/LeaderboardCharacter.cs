using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class LeaderboardCharacter
    {
        [JsonProperty("server_id")]
        public string ServerId = "";

        [JsonProperty("rank")]
        public int Rank;

        [JsonProperty("id")]
        public int Id;

        [JsonProperty("name")]
        public string Name = "";

        [JsonProperty("guild_id")]
        public int GuildId;

        [JsonProperty("attacked_count")]
        public int Attacks;

        [JsonProperty("max_attack_count")]
        public int MaxAttacksAllowed;
    }
}
