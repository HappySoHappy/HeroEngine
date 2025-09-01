using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class LoginBonusRewards
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("status")]
        public int Status; // 1 = unclaimed

        //[JsonProperty("date_key")]
        //public string Date; // "2024-11-26"
        //rewards json string

        public bool IsUnclaimed()
        {
            return Status == 1;
        }
    }
}
