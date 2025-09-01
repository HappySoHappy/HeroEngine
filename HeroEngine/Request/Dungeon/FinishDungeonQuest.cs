using HeroEngine.Framework;
using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Request.Dungeon
{
    public class FinishDungeonQuest : Request
    {
        public FinishDungeonQuest(Account account) : base(account, "finishDungeonQuest")
        {
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

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

            var updateDungeonQuest = JsonConvert.DeserializeObject<DungeonQuest>(JsonConvert.SerializeObject(data.dungeon_quest));
            foreach (var dungeonQuest in game.DungeonQuests)
            {
                if (dungeonQuest.Id != updateDungeonQuest.Id) continue;

                dungeonQuest.Status = updateDungeonQuest.Status;
            }
        }
    }
}
