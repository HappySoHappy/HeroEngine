using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Mission
{
    public class ClaimQuestRewards : Request
    {
        public bool DiscardItem;
        public ClaimQuestRewards(Account account, bool discardItem = false) : base(account, "claimQuestRewards")
        {
            DiscardItem = discardItem;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();
 
            data["discard_item"] = DiscardItem.ToString().ToLower();
            data["refresh_inventory"] = "true";

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
