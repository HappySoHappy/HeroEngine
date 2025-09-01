using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Villain
{
    public class ClaimWorldbossEventRewards : Request
    {
        public int EventId;
        public ClaimWorldbossEventRewards(Account account, int eventId) : base(account, "claimWorldbossEventRewards")
        {
            EventId = eventId;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["worldboss_event_id"] = EventId;
            data["discard_item"] = "false";

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
