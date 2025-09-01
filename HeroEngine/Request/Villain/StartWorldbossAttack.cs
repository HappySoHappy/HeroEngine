using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Villain
{
    public class StartWorldbossAttack : Request
    {
        public int EventId;
        public StartWorldbossAttack(Account account, int eventId) : base(account, "startWorldbossAttack")
        {
            EventId = eventId;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["worldboss_event_id"] = EventId;
            data["iterations"] = 1;

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
