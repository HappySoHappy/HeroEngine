using HeroEngine.Framework;
using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Request.Dungeon
{
    public class RestartDungeon : Request
    {
        public int Dungeon;
        public RestartDungeon(Account account, int dungeon) : base(account, "restartDungeon")
        {
            Dungeon = dungeon;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["dungeon_id"] = Dungeon;

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

            var updateDungeon = JsonConvert.DeserializeObject<Model.Dungeon>(JsonConvert.SerializeObject(data.dungeon));
            foreach (var dungeon in game.Dungeons)
            {
                if (dungeon.Id != updateDungeon.Id) continue;

                dungeon.Status = updateDungeon.Status;
                dungeon.CurrentQuestId = updateDungeon.CurrentQuestId;
                dungeon.ProgressIndex = updateDungeon.ProgressIndex;
                dungeon.Mode = updateDungeon.Mode;
                dungeon.TimeCompleted = updateDungeon.TimeCompleted;
            }

            var updateDungeonQuest = JsonConvert.DeserializeObject<DungeonQuest>(JsonConvert.SerializeObject(data.dungeon_quest));
            foreach (var dungeonQuest in game.DungeonQuests)
            {
                if (dungeonQuest.Id != updateDungeonQuest.Id) continue;

                dungeonQuest.Status = updateDungeonQuest.Status;
                dungeonQuest.BattleId = updateDungeonQuest.BattleId;
                dungeonQuest.Mode = updateDungeonQuest.Mode;
                dungeonQuest.HardmodeDifficulty = updateDungeonQuest.HardmodeDifficulty;
                dungeonQuest.DungeonId = updateDungeonQuest.DungeonId;
            }
        }
    }
}
