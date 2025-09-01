using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.League
{
    public class ClaimMissedLeagueFightsRewards : Request
    {
        public ClaimMissedLeagueFightsRewards(Account account) : base(account, "claimMissedLeagueFightsRewards")
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
