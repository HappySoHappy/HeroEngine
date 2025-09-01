using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request
{
    public class ClaimDailyLoginBonus : Request
    {
        public int BonusId;
        public bool DiscardItem;
        public ClaimDailyLoginBonus(Account account, int bonusId, bool discardItem = false) : base(account, "claimDailyLoginBonus")
        {
            BonusId = bonusId;
            DiscardItem = discardItem;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["id"] = BonusId;
            data["discard_item"] = DiscardItem.ToString().ToLower();

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
            game.UnclaimedLoginRewards = null;
        }
    }
}
