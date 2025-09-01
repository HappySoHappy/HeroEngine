using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.League
{
    public class StartLeagueFight : Request
    {
        public int OpponentId;
        public StartLeagueFight(Account account, int opponentId) : base(account, "startLeagueFight")
        {
            OpponentId = opponentId;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["character_id"] = OpponentId;
            data["use_premium"] = "false";
            data["refresh_opponents"] = "true";

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
