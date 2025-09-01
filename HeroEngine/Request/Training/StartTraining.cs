using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Request.Training
{
    public class StartTraining : Request
    {
        public int TrainingId;
        public StartTraining(Account account, int trainingId) : base(account, "startTraining")
        {
            TrainingId = trainingId;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["training_id"] = $"{TrainingId}";
            data["refresh_trainings"] = "true";

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


            var updateTraining = JsonConvert.DeserializeObject<Model.Training>(JsonConvert.SerializeObject(data.training));
            foreach (var training in game.Trainings!.Where(training => training.Id == updateTraining.Id))
            {
                training.Id = updateTraining.Id;
                training.TimeExpires = updateTraining.TimeExpires;

                if (game.ActiveTraining != null)
                {
                    game.ActiveTraining.RequiredProgress = training.RequiredProgress;
                }
            }
        }
    }
}
