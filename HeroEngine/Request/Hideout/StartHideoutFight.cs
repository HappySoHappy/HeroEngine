using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Hideout
{
    public class StartHideoutFight : Request
    {
        public int Attackers;
        public int Glue;
        public int Stone;
        public StartHideoutFight(Account account, int attackers, int glue, int stone) : base(account, "startHideoutFight")
        {
            Attackers = attackers;
            Glue = glue;
            Stone = stone;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["attacker_count"] = Attackers;
            data["expacted_glue_reward"] = Glue;
            data["expacted_stone_reward"] = Stone;
            data["use_premium"] = "false";

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
