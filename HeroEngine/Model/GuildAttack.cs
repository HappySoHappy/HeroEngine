using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class GuildAttack
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("ts_attack")]
        public long TimeStart;

#pragma warning disable CS8618
        [JsonProperty("character_ids")]
        [JsonConverter(typeof(ArrayStringConverter<int>))]
        public int[] ParticipatingCharacters;
#pragma warning restore CS8618

        public bool IsParticipating(int id)
        {
            return ParticipatingCharacters.Contains(id);
        }
    }
}
