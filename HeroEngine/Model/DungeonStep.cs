using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class DungeonStep
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("story_dungeon_index")]
        public int DungeonIndex;

        [JsonProperty("step_index")]
        public int StepIndex;

        [JsonProperty("status")]
        public int Status;

        [JsonProperty("repeat")]
        public int Repeat;

        [JsonProperty("points_collected")]
        public int PointsCollected;

        [JsonProperty("ts_complete")]
        public int TimeCompleted;

        [JsonProperty("ts_last_attack")]
        public int TimeLastAttack;

        [JsonProperty("battle_ids")]
        public string BattleIds = "";
    }
}
