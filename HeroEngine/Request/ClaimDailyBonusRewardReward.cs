using HeroEngine.Framework;
using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Request
{
    public class ClaimDailyBonusRewardReward : Request
    {
        public int RewardId;
        public bool DiscardItem;
        public ClaimDailyBonusRewardReward(Account account, int rewardId, bool discardItem = false) : base(account, "claimDailyBonusRewardReward")
        {
            RewardId = rewardId;
            DiscardItem = discardItem;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["id"] = RewardId;
            data["discard_item"] = DiscardItem.ToString().ToLower();

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

            if (game.UnlockedBonusRewards != null)
            {
                var updateReward = JsonConvert.DeserializeObject<DungeonQuest>(JsonConvert.SerializeObject(data.daily_bonus_reward));

                foreach (var bonusReward in game.UnlockedBonusRewards)
                {
                    if (bonusReward.Id != updateReward.Id) continue;

                    bonusReward.Status = updateReward.Status;
                }
            }
        }
    }
}
