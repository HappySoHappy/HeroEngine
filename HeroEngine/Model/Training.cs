using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class Training
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("training_cost")]
        public int MotivationCost;

        [JsonProperty("energy")]
        public int Progress;

        [JsonProperty("needed_energy")]
        public int RequiredProgress;

        [JsonProperty("stat_type")]
        public int StatType;

        [JsonProperty("duration")]
        public int Duration;

        [JsonProperty("ts_end")]
        public long TimeExpires;

        [JsonProperty("training_quest_id")]
        public int ActiveQuestId;

        [JsonProperty("claimed_stars")]
        public int ClaimedStars;

#pragma warning disable CS8618
        [JsonProperty("rewards_star_1")]
        [JsonConverter(typeof(JsonStringConverter))]
        public Reward FirstMilestoneRewards;

        [JsonProperty("rewards_star_2")]
        [JsonConverter(typeof(JsonStringConverter))]
        public Reward SecondMilestoneRewards;

        [JsonProperty("rewards_star_3")]
        [JsonConverter(typeof(JsonStringConverter))]
        public Reward ThirdMilestoneRewards;
#pragma warning restore CS8618

        [JsonProperty("stat_points_star_1")]
        public int FirstMilestoneStats;

        [JsonProperty("stat_points_star_2")]
        public int SecondMilestoneStats;

        [JsonProperty("stat_points_star_3")]
        public int ThirdMilestoneStats;

        public string GetTrainingType()
        {
            switch (StatType)
            {
                case 1:
                    return "Stamina";
                case 2:
                    return "Strength";
                case 3:
                    return "Critical";
                case 4:
                    return "Dodge";
                default:
                    return "unknown";
            }
        }

        public bool IsStaminaTraining()
        {
            return StatType == 1;
        }

        public bool IsStrengthTraining()
        {
            return StatType == 2;
        }

        public bool IsCriticalTraining()
        {
            return StatType == 3;
        }

        public bool IsDodgeTraining()
        {
            return StatType == 4;
        }
    }
}
