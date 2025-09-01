using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class DungeonQuest
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("identifier")]
        public string Identifier = "";

        [JsonProperty("status")]
        public int Status;

        [JsonProperty("battle_id")]
        public int BattleId;

        [JsonProperty("mode")]
        public int Mode;

        [JsonProperty("hardmode_difficulty")]
        public int HardmodeDifficulty;

        [JsonProperty("dungeon_id")]
        public int DungeonId;
    }
}
