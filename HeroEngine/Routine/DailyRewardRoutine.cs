using HeroEngine.Persistance;
using HeroEngine.Request;
using HeroEngine.Util;

namespace HeroEngine.Routine
{
    public class DailyRewardRoutine : Routine<RoutineResult>
    {
        /* daily_bonus_lookup
         * id
         * ts_last_reset
         * herobook_daily_completed
         * duel_battle_won
         * league_battle_won
         * hideout_battle_won
         * quest_energy_spent
         */

        /*
         * daily_bonus_reward_data
         * - 1 (alarm call quest complete)
         *     keys 3
         *     
         * - 2 (duel fights)
         *     keys 10, 20, 50
         *     
         * - 3 (league fights)
         *     keys 5, 10, 20
         *     
         * - 4 (hideout fights)
         *     keys 5, 10, 20
         *     
         * - 5 (spent mission energy)
         *     keys 100, 200, 300 object {reward_type, reward_value, reward_identifier}
         * 
         * key is the value required to receive reward
         */

        protected ExecutionConfiguration _config;
        public DailyRewardRoutine(Account account, ExecutionConfiguration config) : base(account)
        {
            _config = config;
        }

        public override bool Execute(out RoutineResult result, out string error)
        {
            if (_account.HeroZero == null || _account.HeroZero.Data == null)
            {
                result = RoutineResult.Uninitialized;
                error = "uninitialized";
                return false;
            }

            var data = _account.HeroZero.Data;

            if (data.UnlockedBonusRewards?.Count > 0)
            {
                foreach (var bonusReward in data.UnlockedBonusRewards.Where(bonus => bonus.IsAvailable()))
                {
                    if (new ClaimDailyBonusRewardReward(_account, bonusReward.Id).Execute(out var claimBonusData, out string claimBonusError))
                    {
                        ClaimDailyBonusRewardReward.Update(_account, claimBonusData);
                        _account.Logger.Info($"Claimed bonus reward {bonusReward.Id} from {UnixTime.Format(UnixTime.Since(bonusReward.TimeUnlocked), false)} ago");
                        //also add contents of it to log
                    } else
                    {
                        _account.Logger.Warn($"Unable to claim bonus reward {bonusReward.Id}, {claimBonusError}");
                    }
                }
            }

            result = RoutineResult.Finished;
            error = "";
            return true;
        }

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            return new DailyRewardRoutine(account, config).Execute(out result, out error);
        }
    }
}
