using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request
{
    public class SellInventoryItem : Request
    {
        public int ItemId;
        public SellInventoryItem(Account account, int itemId) : base(account, "sellInventoryItem")
        {
            ItemId = itemId;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["item_id"] = ItemId;

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
