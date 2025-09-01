using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Training
{
    public class ClaimTrainingStar : Request
    {
        public bool DiscardItem;
        public ClaimTrainingStar(Account account, bool discardItem = false) : base(account, "claimTrainingStar")
        {
            DiscardItem = discardItem;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["discard_item"] = DiscardItem.ToString().ToLower();

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
