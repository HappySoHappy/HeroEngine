using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Guild
{
    public class JoinGuildBattle : Request
    {
        public bool Attack;
        public JoinGuildBattle(Account account, bool attack) : base(account, "joinGuildBattle")
        {
            Attack = attack;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["attack"] = Attack.ToString().ToLower();

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
