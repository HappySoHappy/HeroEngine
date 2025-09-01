using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class Mission
    {
        [JsonProperty("id")]
        public int Id;

#pragma warning disable CS8618
        [JsonProperty("identifier")]
        public string Identifier;
#pragma warning restore CS8618

        [JsonProperty("type")]
        public int Type;

        [JsonProperty("duration")]
        public int Duration;

        [JsonProperty("ts_complete")]
        public long TimeComplete;

        [JsonProperty("energy_cost")]
        public int EnergyCost;

#pragma warning disable CS8618
        [JsonProperty("rewards")]
        [JsonConverter(typeof(JsonStringConverter))]
        public Reward Rewards;
#pragma warning restore CS8618

        public string GetMissionType()
        {
            switch (Type)
            {
                case 1:
                    return "Timed";
                case 2:
                    return "Fight";
                default:
                    return "unknown";
            }
        }

        public bool IsFightMission()
        {
            return Type == 2;
        }

        public bool IsEventMission()
        {
            return !Identifier.StartsWith("quest_stage");
        }
    }
}
