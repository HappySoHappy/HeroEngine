using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class TreasureEvent
    {
        [JsonProperty("id")]
        public int Id;

#pragma warning disable CS8618
        [JsonProperty("identifier")]
        public string Identifier;
#pragma warning restore CS8618

        [JsonProperty("event_reveal_items")]
        public int Shovels;

        [JsonProperty("event_tokens")]
        public int DiscoveredTreasures;

        [JsonProperty("ts_reveal_item_collected")]
        public int TimeShovelsCollected;

#pragma warning disable CS8618
        [JsonProperty("end_date")]
        [JsonConverter(typeof(DateStringConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime DateExpires;
#pragma warning restore CS8618
    }
}
