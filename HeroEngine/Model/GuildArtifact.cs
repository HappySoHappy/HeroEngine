using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class GuildArtifact
    {
        [JsonProperty("asset_identifier")]
        public string Identifier = "";

        [JsonProperty("type")]
        public int Type;

        [JsonProperty("value")]
        public int Value;

        //typ 1 = odwaga, liga ataki
        //typ 2 = misje energia
        //typ 3 = treny
        //typ 4 = nagroda za walki

        public bool IsCourageBooster()
        {
            return Type == 1;
        }

        public bool IsMissionEnergy()
        {
            return Type == 2;
        }

        public bool IsTrainingMotivation()
        {
            return Type == 3;
        }

        public bool IsAttackRewardBooster()
        {
            return Type == 4;
        }
    }
}
