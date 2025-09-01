using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class GuildCompetition
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("ts_end")]
        public long TimeEnds;

        [JsonProperty("ts_start")]
        public long TimeBegins;

        [JsonProperty("identifier")]
        public string Identifier = "";
    }
}
