using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class User
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("ts_creation")]
        public long TimeCreated;

        [JsonProperty("login_count")]
        public long LoginCount;

#pragma warning disable CS8618
        [JsonProperty("session_id")]
        public string SessionId;
#pragma warning restore CS8618

        [JsonProperty("premium_currency")]
        public int Donuts;
    }
}
