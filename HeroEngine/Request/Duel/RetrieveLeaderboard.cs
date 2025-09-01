using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Duel
{
    public class RetrieveLeaderboard : Request
    {
        public int? Rank;
        public bool LevelSort;
        public bool SameLocale;
        public RetrieveLeaderboard(Account account, int? rank, bool levelSort, bool sameLocale = false) : base(account, "retrieveLeaderboard")
        {
            Rank = rank;
            LevelSort = levelSort;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            if (Rank != null) // 0 to find our character
            {
                data["rank"] = Rank;
            }
            data["level_sort"] = LevelSort.ToString().ToLower();
            data["same_locale"] = SameLocale.ToString().ToLower();
            data["server_id"] = "";

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
