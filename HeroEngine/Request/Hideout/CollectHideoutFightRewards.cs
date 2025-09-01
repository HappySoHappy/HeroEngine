using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Hideout
{
    public class CollectHideoutFightRewards : Request
    {
        public int BattleId;
        public CollectHideoutFightRewards(Account account, int battleId) : base(account, "collectHideoutFightRewards")
        {
            BattleId = battleId;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["hideout_battle_id"] = BattleId;

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
