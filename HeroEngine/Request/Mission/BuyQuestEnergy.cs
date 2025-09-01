using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Mission
{
    public class BuyQuestEnergy : Request
    {
        public BuyQuestEnergy(Account account) : base(account, "buyQuestEnergy")
        {
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["use_premium"] = "false";

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
