using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Hideout
{
    public class OpenOpticalChangeChests : Request
    {
        public List<int> ChestIds;
        public OpenOpticalChangeChests(Account account, List<int> chestIds) : base(account, "openOpticalChangeChests")
        {
            ChestIds = chestIds;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["opticalChangeChestIds"] = "%5B" + string.Join("%2C", ChestIds) + "%5D";

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
