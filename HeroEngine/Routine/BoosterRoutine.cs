using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Request.Booster;
using HeroEngine.Util;
using static HeroEngine.Persistance.ExecutionConfiguration;

namespace HeroEngine.Routine
{
    public class BoosterRoutine : Routine<RoutineResult>
    {
        protected ExecutionConfiguration _config;
        public BoosterRoutine(Account account, ExecutionConfiguration config) : base(account)
        {
            _config = config;
        }

        public override bool Execute(out RoutineResult result, out string error)
        {
            var data = _account.HeroZero!.Data;

            #region Quest Booster
            var currentQuestBooster = data.Character.QuestBooster switch
            {
                "quest_booster1" => QuestBoosterSelect.Small,
                "quest_booster2" => QuestBoosterSelect.Medium,
                "quest_booster3" => QuestBoosterSelect.Premium,
                _ => QuestBoosterSelect.None
            };

            string preferredQuestBoosterId = _config.QuestBoosterPreferredSelect switch
            {
                QuestBoosterSelect.Small => "booster_quest1",
                QuestBoosterSelect.Medium => "booster_quest2",
                QuestBoosterSelect.Premium => "booster_quest3",
                _ => ""
            };

            switch (_config.QuestBoosterPreferredSelect)
            {
                case QuestBoosterSelect.Small:
                case QuestBoosterSelect.Medium:
                    bool small = _config.QuestBoosterPreferredSelect == QuestBoosterSelect.Small;
                    int cost = CalculateBoosterCost(data.Character.Level, small);

                    if (UnixTime.Until(data.Character.TimeQuestBoosterExpires) <= (currentQuestBooster == _config.QuestBoosterPreferredSelect ? 3600 : 0)
                        && data.Character.GoldCoins >= cost)
                    {
                        if (new BuyBooster(_account, preferredQuestBoosterId).Execute(out var buyBoosterData, out string buyBoosterError))
                        {
                            BuyBooster.Update(_account, buyBoosterData);
                            _account.Logger.Info($"Bought {(small ? "10%" : "25%")} quest booster for {cost} gold");
                        } else
                        {
                            _account.Logger.Warn($"Unable to buy quest booster, {buyBoosterError}");
                        }
                    }

                    break;

                case QuestBoosterSelect.Premium:
                    if (UnixTime.Until(data.Character.TimeQuestBoosterExpires) <= 3600 // override other boosters
                        && data.User.Donuts >= HeroZero.Constants.BoosterLargePremiumCost)
                    {
                        if (new BuyBooster(_account, preferredQuestBoosterId).Execute(out var buyBoosterData, out string buyBoosterError))
                        {
                            BuyBooster.Update(_account, buyBoosterData);
                            _account.Logger.Info($"Bought 50% quest booster for {HeroZero.Constants.BoosterLargePremiumCost} donuts");
                        }
                        else
                        {
                            _account.Logger.Warn($"Unable to buy quest booster, {buyBoosterError}");
                        }
                    }

                    break;
            }
            #endregion

            #region Stats Booster
            var currentStatBooster = data.Character.StatsBooster switch
            {
                "booster_stats1" => StatBoosterSelect.Small,
                "booster_stats2" => StatBoosterSelect.Medium,
                "booster_stats3" => StatBoosterSelect.Premium,
                _ => StatBoosterSelect.None
            };

            string preferredStatBoosterId = _config.StatBoosterPreferredSelect switch
            {
                StatBoosterSelect.Small => "booster_stats1",
                StatBoosterSelect.Medium => "booster_stats2",
                StatBoosterSelect.Premium => "booster_stats3",
                _ => ""
            };

            switch (_config.StatBoosterPreferredSelect)
            {
                case StatBoosterSelect.Small:
                case StatBoosterSelect.Medium:
                    bool small = _config.StatBoosterPreferredSelect == StatBoosterSelect.Small;
                    int cost = CalculateBoosterCost(data.Character.Level, small);

                    if (UnixTime.Until(data.Character.TimeStatsBoosterExpires) <= (currentStatBooster == _config.StatBoosterPreferredSelect ? 3600 : 0)
                        && data.Character.GoldCoins >= cost)
                    {
                        if (new BuyBooster(_account, preferredStatBoosterId).Execute(out var buyBoosterData, out string buyBoosterError))
                        {
                            BuyBooster.Update(_account, buyBoosterData);
                            _account.Logger.Info($"Bought {(small ? "10%" : "25%")} stats booster for {cost} gold");
                        }
                        else
                        {
                            _account.Logger.Warn($"Unable to buy stats booster, {buyBoosterError}");
                        }
                    }

                    break;

                case StatBoosterSelect.Premium:
                    if (UnixTime.Until(data.Character.TimeStatsBoosterExpires) <= 3600 // override other boosters
                        && data.User.Donuts >= HeroZero.Constants.BoosterLargePremiumCost)
                    {
                        if (new BuyBooster(_account, preferredStatBoosterId).Execute(out var buyBoosterData, out string buyBoosterError))
                        {
                            BuyBooster.Update(_account, buyBoosterData);
                            _account.Logger.Info($"Bought 50% stats booster for {HeroZero.Constants.BoosterLargePremiumCost} donuts");
                        }
                        else
                        {
                            _account.Logger.Warn($"Unable to buy stats booster, {buyBoosterError}");
                        }
                    }

                    break;
            }
            #endregion

            #region Work Booster
            var currentWorkBooster = data.Character.WorkBooster switch
            {
                "booster_work1" => WorkBoosterSelect.Small,
                "booster_work2" => WorkBoosterSelect.Medium,
                "booster_work3" => WorkBoosterSelect.Premium,
                _ => WorkBoosterSelect.None
            };

            string preferredWorkBoosterId = _config.WorkBoosterPreferredSelect switch
            {
                WorkBoosterSelect.Small => "booster_work1",
                WorkBoosterSelect.Medium => "booster_work2",
                WorkBoosterSelect.Premium => "booster_work3",
                _ => ""
            };

            switch (_config.WorkBoosterPreferredSelect)
            {
                case WorkBoosterSelect.Small:
                case WorkBoosterSelect.Medium:
                    bool small = _config.WorkBoosterPreferredSelect == WorkBoosterSelect.Small;
                    int cost = CalculateBoosterCost(data.Character.Level, small);

                    if (UnixTime.Until(data.Character.TimeWorkBoosterExpires) <= (currentWorkBooster == _config.WorkBoosterPreferredSelect ? 3600 : 0)
                        && data.Character.GoldCoins >= cost)
                    {
                        if (new BuyBooster(_account, preferredWorkBoosterId).Execute(out var buyBoosterData, out string buyBoosterError))
                        {
                            BuyBooster.Update(_account, buyBoosterData);
                            _account.Logger.Info($"Bought {(small ? "10%" : "25%")} work booster for {cost} gold");
                        }
                        else
                        {
                            _account.Logger.Warn($"Unable to buy work booster, {buyBoosterError}");
                        }
                    }

                    break;

                case WorkBoosterSelect.Premium:
                    if (UnixTime.Until(data.Character.TimeWorkBoosterExpires) <= 3600 // override other boosters
                        && data.User.Donuts >= HeroZero.Constants.BoosterLargePremiumCost)
                    {
                        if (new BuyBooster(_account, preferredWorkBoosterId).Execute(out var buyBoosterData, out string buyBoosterError))
                        {
                            BuyBooster.Update(_account, buyBoosterData);
                            _account.Logger.Info($"Bought 50% work booster for {HeroZero.Constants.BoosterLargePremiumCost} donuts");
                        }
                        else
                        {
                            _account.Logger.Warn($"Unable to buy work booster, {buyBoosterError}");
                        }
                    }

                    break;
            }
            #endregion

            #region League Booster
            var currentLeagueBooster = data.Character.LeagueBooster switch
            {
                "booster_league1" => LeagueBoosterSelect.Medium,
                "booster_league2" => LeagueBoosterSelect.Premium,
                _ => LeagueBoosterSelect.None
            };

            string preferredLeagueBoosterId = _config.LeagueBoosterPreferredSelect switch
            {
                LeagueBoosterSelect.Medium => "booster_league1",
                LeagueBoosterSelect.Premium => "booster_league2",
                _ => ""
            };

            switch (_config.LeagueBoosterPreferredSelect)
            {
                case LeagueBoosterSelect.Medium:
                    int cost = CalculateBoosterCost(data.Character.Level, false);

                    if (UnixTime.Until(data.Character.TimeLeagueBoosterExpires) <= (currentLeagueBooster == _config.LeagueBoosterPreferredSelect ? 3600 : 0)
                        && data.Character.GoldCoins >= cost)
                    {
                        if (new BuyLeagueBooster(_account, false).Execute(out var buyBoosterData, out string buyBoosterError))
                        {
                            BuyLeagueBooster.Update(_account, buyBoosterData);
                            _account.Logger.Info($"Bought small league booster for {cost} gold");
                        }
                        else
                        {
                            _account.Logger.Warn($"Unable to buy league booster, {buyBoosterError}");
                        }
                    }

                    break;

                case LeagueBoosterSelect.Premium:
                    if (UnixTime.Until(data.Character.TimeLeagueBoosterExpires) <= 3600 // override other boosters
                        && data.User.Donuts >= HeroZero.Constants.BoosterLargePremiumCost)
                    {
                        if (new BuyLeagueBooster(_account, true).Execute(out var buyBoosterData, out string buyBoosterError))
                        {
                            BuyLeagueBooster.Update(_account, buyBoosterData);
                            _account.Logger.Info($"Bought large league booster for {HeroZero.Constants.BoosterLargePremiumCost} donuts");
                        }
                        else
                        {
                            _account.Logger.Warn($"Unable to buy league booster, {buyBoosterError}");
                        }
                    }

                    break;
            }
            #endregion

            result = RoutineResult.Finished;
            error = "";
            return true;
        }

        private int CalculateBoosterCost(int level, bool small = false)
        {
            int boosterLevel = (int)Math.Ceiling((level + 1) / 10.0);
            double boosterTime = small ? HeroZero.Constants.BoosterSmallCost : HeroZero.Constants.BoosterMediumCost;

            return (int)Math.Round(
                Math.Ceiling(
                    (
                        HeroZero.Constants.CoinBase +
                        HeroZero.Constants.CoinScale *
                        Math.Pow(
                            HeroZero.Constants.CoinLevelScale * (10 * boosterLevel - 9),
                            HeroZero.Constants.CoinLevelExperience
                        )
                    ) * boosterTime / HeroZero.Constants.BoosterCoinCostStep
                ) * HeroZero.Constants.BoosterCoinCostStep
            );
        }

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            return new BoosterRoutine(account, config).Execute(out result, out error);
        }
    }
}
