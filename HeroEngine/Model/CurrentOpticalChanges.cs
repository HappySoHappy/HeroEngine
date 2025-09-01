using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class CurrentOpticalChanges
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("ts_last_free_chest")]
        public long TimeLastFreeChest;

        //TODO: implement proper deserialization of JSON-object-array strings
        /*[JsonProperty("available_chests")]
        [JsonConverter(typeof(ArrayStringConverter<OpticalChangeChest>))]
        public List<OpticalChangeChest> ChestsAvailable;*/

        [JsonProperty("available_chests")]
        public string ChestsAvailable = "";
    }
}
