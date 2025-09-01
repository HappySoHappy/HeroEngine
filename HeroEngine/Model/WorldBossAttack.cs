using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class WorldBossAttack
    {
        [JsonProperty("worldboss_event_id")]
        public int Id;

        //[JsonProperty("worldboss_event_id")]
        //public int WorldBossId;

        [JsonProperty("status")]
        public int Status;

        [JsonProperty("duration")]
        public int Duration;

        [JsonProperty("ts_complete")]
        public long TimeComplete;
    }
}
