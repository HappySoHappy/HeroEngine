using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Treasure
{
    public class ClaimFreeTreasureRevealItems : Request
    {
        public ClaimFreeTreasureRevealItems(Account account) : base(account, "claimFreeTreasureRevealItems")
        {
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

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
