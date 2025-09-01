using HeroEngine.Framework;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class Guild
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("pending_guild_battle_attack_id")]
        public int PendingAttackId;

        [JsonProperty("pending_guild_battle_defense_id")]
        public int PendingDefenseId;

        [JsonProperty("pending_guild_dungeon_battle_attack_id")]
        public int PendingDungeonId;

        [JsonProperty("artifact_ids")]
        [JsonConverter(typeof(ArrayStringConverter<int>))]
        public int[] ArtifactIds; //[1,2,3,4 etc...]

        //typ 1 = odwaga, liga ataki
        //typ 2 = misje energia
        //typ 3 = treny
        //typ 4 = nagroda za walki

        public List<GuildArtifact> GetArtifacts()
        {
            List<GuildArtifact> artifacts = new List<GuildArtifact>();

            foreach (int id in ArtifactIds)
            {
                artifacts.Add(HeroZero.Constants.GuildArtifacts[$"{id}"]);
            }

            return artifacts;
        }
    }
}
