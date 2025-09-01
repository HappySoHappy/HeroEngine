using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Guild
{
    public class ClaimGuildDungeonBattleReward : Request
    {
        public int Id;
        public bool Discard;
        public ClaimGuildDungeonBattleReward(Account account, int id, bool discard = false) : base(account, "claimGuildDungeonBattleReward")
        {
            Id = id;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["discard_item"] = Discard.ToString().ToLower();
            data["guild_dungeon_battle_id"] = Id;

            return data;
        }

        public static void Update(Account account, dynamic data)
        {
            if (data == null) return;

            var hz = account.HeroZero;
            if (hz == null) return;

            var game = hz.Data;
            if (game == null) return;

            JsonPropertyUpdater.UpdateFields(game, data);
        }
    }
}
