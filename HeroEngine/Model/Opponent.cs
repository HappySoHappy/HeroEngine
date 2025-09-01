using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class Opponent
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("name")]
        public string Name = "";

        [JsonProperty("has_missile")]
        public bool HasThrowables;

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
