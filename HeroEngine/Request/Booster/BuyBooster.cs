using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Booster
{
    public class BuyBooster : Request
    {
        public string BoosterId;
        public BuyBooster(Account account, string boosterId) : base(account, "buyBooster")
        {
            BoosterId = boosterId;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["id"] = BoosterId;

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

        //booster_quest1 - 10%
        //booster_quest2 - 25%
        //booster_quest3 - 50% (premium)

        //booster_stats1

        //booster_work1
    }
}
