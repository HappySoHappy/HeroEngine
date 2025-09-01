using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Villain
{
    public class CheckForWorldbossAttackComplete : Request
    {
        public CheckForWorldbossAttackComplete(Account account) : base(account, "checkForWorldbossAttackComplete")
        {
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["quest_id"] = 0;

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
