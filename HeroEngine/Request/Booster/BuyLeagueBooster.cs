using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Booster
{
    public class BuyLeagueBooster : Request
    {
        public bool Premium;
        public BuyLeagueBooster(Account account, bool premium) : base(account, "buyLeagueBooster")
        {
            Premium = premium;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["id"] = Premium ? "booster_league2" : "booster_league1";

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
