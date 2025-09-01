using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class Reward //"{\"coins\":22600,\"xp\":3710}"
    {
        [JsonProperty("coins")]
        public int Gold;

        [JsonProperty("xp")]
        public int Experience;

        [JsonProperty("item")]
        public int ItemId;

        [JsonProperty("training_progress")]
        public int TrainingProgress;

        [JsonProperty("training_sessions")]
        public int TrainingMotivation;

        [JsonProperty("quest_energy")]
        public int MissionEnergy;

        [JsonProperty("statPoints")]
        public int StatPoints;

        [JsonProperty("hideout_glue")]
        public int HideoutGlue;

        [JsonProperty("hideout_stone")]
        public int HideoutZeronite;
    }
}
