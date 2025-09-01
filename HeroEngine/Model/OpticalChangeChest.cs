using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class OpticalChangeChest
    {
        [JsonProperty("free")]
        public bool Free { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
