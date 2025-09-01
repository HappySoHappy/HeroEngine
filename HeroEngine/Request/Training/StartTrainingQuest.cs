using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Training
{
    public class StartTrainingQuest : Request
    {
        public int TrainingQuestId;
        public StartTrainingQuest(Account account, int trainingQuestId) : base(account, "startTrainingQuest")
        {
            TrainingQuestId = trainingQuestId;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["training_quest_id"] = $"{TrainingQuestId}";
            data["training_ids"] = "0";

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
