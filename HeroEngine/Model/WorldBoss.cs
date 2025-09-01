using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class WorldBoss
    {
        [JsonProperty("worldboss_event_id")]
        public int Id;

        [JsonProperty("ranking")]
        public int Rank;
    }
}
