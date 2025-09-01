using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class DungeonLookup
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("story_dungeon_step_ids")]
        public string StepIds = ""; //"[20720,45563,45564]"

        [JsonProperty("story_dungeon_steps")]
        public string Steps = ""; //"{\"1\":5,\"2\":5,\"3\":1}"
    }
}
