using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Hideout
{
    public class RefreshHideoutOpponent : Request
    {
        public bool Premium;
        public RefreshHideoutOpponent(Account account, bool premium) : base(account, "refreshHideoutOpponent")
        {
            Premium = premium;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["use_premium"] = Premium.ToString().ToLower();

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
