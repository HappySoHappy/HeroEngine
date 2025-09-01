using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class WorldBossEvent
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("ts_finished")]
        public long TimeComplete; // 0 if not

        [JsonProperty("npc_hitpoints_current")]
        public int Health;

        [JsonProperty("winning_attacker_id")]
        public int WinnerId; // 0 if not finished
    }
}
