using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class DailyBonusReward
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("ts_creation")]
        public long TimeUnlocked;

        [JsonProperty("status")]
        public int Status;

        [JsonProperty("type")]
        public int Type;

        [JsonProperty("value")]
        public int Value;

#pragma warning disable CS8618
        [JsonProperty("rewards")]
        [JsonConverter(typeof(JsonStringConverter))]
        public Reward Rewards;
#pragma warning restore CS8618

        public bool IsAvailable() => Type == 1;
    }
}
