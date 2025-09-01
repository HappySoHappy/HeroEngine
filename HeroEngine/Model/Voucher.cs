using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class Voucher
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("code")]
        public string Code = "";

#pragma warning disable CS8618
        [JsonProperty("rewards")]
        [JsonConverter(typeof(JsonStringConverter))]
        public Reward Rewards;
#pragma warning restore CS8618

        [JsonProperty("ts_end")]
        public long TimeExpires;

        public bool IsExpired()
        {
            return UnixTime.Since(TimeExpires) > 0;
        }

        public bool IsHideoutVoucher()
        {
            return Code.Contains("glue") || Code.Contains("zeronit");
        }

        public bool IsEnergyVoucher()
        {
            return Code.Contains("energy");
        }

        public bool IsTrainingVoucher()
        {
            return Code.Contains("training");
        }
    }
}
