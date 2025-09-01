using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class GuildMember
    {
        [JsonProperty("id")]
        public int CharacterId;

        [JsonProperty("user_id")]
        public int UserId;

        [JsonProperty("name")]
        public string Name = "";

        [JsonProperty("ts_guild_joined")]
        public long TimeGuildJoined;

        [JsonProperty("stat_total_stamina")]
        public int Stamina;

        [JsonProperty("stat_total_strength")]
        public int Strength;

        [JsonProperty("stat_total_critical_rating")]
        public int CriticalRating;

        [JsonProperty("stat_total_dodge_rating")]
        public int DodgeRating;
    }
}
