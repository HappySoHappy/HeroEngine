using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class HideoutOpponent
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("name")]
        public string Name = "";

        [JsonProperty("defender_units")]
        public int DefenderUnits;

        [JsonProperty("wall_level")]
        public int WallLevel;

        [JsonProperty("barracks_level")]
        public int BarracksLevel;

#pragma warning disable CS8618
        [JsonProperty("rewards")]
        [JsonConverter(typeof(JsonStringConverter))]
        public Reward Rewards;
#pragma warning restore CS8618

#pragma warning disable CS8618
        [JsonProperty("bonus_rewards")]
        [JsonConverter(typeof(JsonStringConverter))]
        public Reward? BonusRewards;
#pragma warning restore CS8618
    }
}
