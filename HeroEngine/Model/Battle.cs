using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class Battle
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("winner")]
        public string Winner = "";
    }
}
