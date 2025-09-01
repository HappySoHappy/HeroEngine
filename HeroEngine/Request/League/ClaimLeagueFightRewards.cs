using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.League
{
    public class ClaimLeagueFightRewards : Request
    {
        public bool DiscardItem;
        public ClaimLeagueFightRewards(Account account, bool discardItem = false) : base(account, "claimLeagueFightRewards")
        {
            DiscardItem = discardItem;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

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
        }
    }
}
