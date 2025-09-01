using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request
{
    public class GetMissedLeagueFights : Request
    {
        public bool History;
        public GetMissedLeagueFights(Account account, bool history = false) : base(account, "getMissedLeagueFights")
        {
            History = history;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["history"] = History.ToString().ToLower();

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
