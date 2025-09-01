using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class TrainingQuest
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("energy_cost")]
        public int EnergyCost;

#pragma warning disable CS8618
        [JsonProperty("rewards")]
        [JsonConverter(typeof(JsonStringConverter))]
        public Reward Rewards;
#pragma warning restore CS8618
    }
}
