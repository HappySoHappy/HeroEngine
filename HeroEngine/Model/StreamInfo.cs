using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class StreamInfo
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("type")]
        public string Type = "";

        [JsonProperty("unread")]
        public int Unread;

        public bool IsMessage()
        {
            return Type == "p";
        }

        public bool IsBatteryRequest()
        {
            return Type == "r";
        }

        public bool IsSystemMessage()
        {
            return Type == "s";
        }

        public bool IsCoupon()
        {
            return Type == "v";
        }
    }
}
