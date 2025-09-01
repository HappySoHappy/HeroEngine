using HeroEngine.Framework;
using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Request;
using HeroEngine.Request.Mission;
using HeroEngine.Request.Villain;
using HeroEngine.Util;

namespace HeroEngine.Routine
{
    public class MissionRoutine : Routine<RoutineResult>
    {
        protected ExecutionConfiguration _config;
        public MissionRoutine(Account account, ExecutionConfiguration config) : base(account)
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

            if (!_config.Missions)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            var data = _account.HeroZero!.Data;

            int energy = GetTotalEnergy();
            if (energy <= 0)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            if (data.ActiveWorldBossAttack != null && UnixTime.Since(data.ActiveWorldBossAttack.TimeComplete) > 0)
            {
                _account.Logger.Info($"Found finished worldboss with id: {data.ActiveWorldBossAttack.Id} and status {data.ActiveWorldBossAttack.Status}");
                if (new CheckForWorldbossAttackComplete(_account).Execute(out var checkData, out string checkError))
                {
                    _account.Logger.Info("Check worldboss attack is complete");
                    CheckForWorldbossAttackComplete.Update(_account, checkData);
                }
                else
                {
                    switch (checkError)
                    {
                        case "errFightWorldbossEventInvalidStatus":
                        case "errCheckForWorldbossAttackCompleteNoActiveWorldbossAttack":
                            break;
                        default:
                            _account.Logger.Warn($"Unable to check worldboss attack complete, {checkError}");
                            break;
                    }
                }

                if (new FinishWorldbossAttack(_account, data.ActiveWorldBossAttack.Id).Execute(out var finishData, out string finishError))
                {
                    _account.Logger.Info("finish worldboss attack");
                    FinishWorldbossAttack.Update(_account, finishData);
                    data.ActiveWorldBossAttack = null;
                }
                else
                {
                    _account.Logger.Warn($"Unable to finish worldboss attack {data.ActiveWorldBossAttack.Id}, {finishError}");
                }
            }

            var defaultMission = GetFallbackMission();
           if (defaultMission.EnergyCost > energy && data.Character.ActiveMissionId == 0)
           {
                var missionVouchers = data.Vouchers
                       .Where(v => v.IsEnergyVoucher() && !v.IsExpired())
                       .OrderBy(v => v.TimeExpires)
                       .ThenByDescending(v => v.Rewards.MissionEnergy);

               if (_config.MissionCouponPreferredSelect != ExecutionConfiguration.MissionCouponSelect.None && missionVouchers.Any())
               {
                    var voucher = missionVouchers.First();

                    int missionTime = voucher.Rewards.MissionEnergy * 180;
                    if (UnixTime.UntilMidnight() > missionTime)
                    {
                        if (new RedeemVoucher(_account, voucher.Code).Execute(out var voucherData, out string voucherError))
                        {
                            RedeemVoucher.Update(_account, voucherData);
                            _account.Logger.Info($"Claimed energy voucher with {voucher.Rewards.MissionEnergy} energy");
                        }
                        else
                        {
                            _account.InternalState!.IsTrainingVoucherFailed = true;
                            _account.Logger.Warn("Unable to claim energy voucher, " + voucherError);
                        }
                    }
               }
               else
               {
                   //we dont have voucher for next mission, and there is no mission running to finish
                   result = RoutineResult.Finished;
                   error = "";
                   return true;
               }
           }

            if (data.Character.ActiveMissionId == 0)
            {
                var preferredMission = GetPreferredMission();
                if (preferredMission == null)
                {
                    result = RoutineResult.Sleeping;
                    error = "";
                    return true;
                }

                if (preferredMission.EnergyCost > data.Character.MissionEnergy && GetEnergyRefills() > 0)
                {
                    int refillCost = GetEnergyRefillCost();
                    if (data.Character.GoldCoins >= refillCost)
                    {
                        if (new BuyQuestEnergy(_account).Execute(out var energyData, out string energyError))
                        {
                            BuyQuestEnergy.Update(_account, energyData);
                            _account.Logger.Info($"Bought mission energy for {refillCost} coins");
                        } else
                        {
                            _account.Logger.Warn($"Unable to buy mission energy: {energyError}");

                            result = RoutineResult.UnhandledError;
                            error = energyError;
                            return false;
                        }
                    } else
                    {
                        result = RoutineResult.Sleeping;
                        error = "";
                        return true;
                    }
                }

                var chosenMission = preferredMission.EnergyCost <= data.Character.MissionEnergy ? preferredMission : defaultMission;
                if (data.Character.MissionEnergy >= chosenMission.EnergyCost)
                {
                    if (new StartQuest(_account, chosenMission.Id).Execute(out var startData, out string startError))
                    {
                        StartQuest.Update(_account, startData);
                        _account.Logger.Info($"Started {UnixTime.Format(chosenMission.Duration, false)} {chosenMission.GetMissionType()} mission for {chosenMission.EnergyCost} energy");
                        _account.Logger.Info($"Currently there is {GetTotalEnergy()} mission energy remaining for today");
                    } else
                    {
                        //errRemoveQuestEnergyNotEnough
                        //errStartQuestActiveDuelFound
                        //errStartQuestActiveLeagueFightFound

                        _account.Logger.Warn($"Unable to start mission: {startError}");

                        result = RoutineResult.UnhandledError;
                        error = startError;
                        return false;
                    }
                }
            }

            var activeMission = GetActiveMission();
            if (activeMission != null)
            {
                var remaining = TimeSpan.FromSeconds(UnixTime.Until(activeMission.TimeComplete) + 1);
                if (remaining > TimeSpan.Zero)
                {
                    result = RoutineResult.Sleeping;
                    error = "";
                    return true;
                }

                if (new CheckQuestComplete(_account).Execute(out var checkData, out string checkError))
                {
                    CheckQuestComplete.Update(_account, checkData);
                    _account.Logger.Info($"Check if mission is complete (success, mission is complete)");
                }
                else
                {
                    result = RoutineResult.UnhandledError;
                    error = checkError;

                    _account.Logger.Warn($"Unable to check if mission is complete: {checkError}");

                    //errFinishNotYetCompleted
                    switch (checkError)
                    {
                        case "errFinishInvalidStatus":
                            //claim quest rewards
                            break;

                        case "errUserNotAuthorized":
                            result = RoutineResult.Unauthorized;
                            return false;

                        case "errRequestMaintenance":
                            result = RoutineResult.Maintenance;
                            return false;

                        case "errRequestOutdatedClientVersion":
                            result = RoutineResult.OutdatedVersion;
                            return false;

                        case "errServerStatus400": // bad request
                        case "errServerStatus403": // forbidden - introduced by cloudflare
                        case "errServerStatus429": // too many requests
                        case "errRequestBlocked":
                            result = RoutineResult.Blocked;
                            return false;
                    }
                }

                if (new ClaimQuestRewards(_account, !data.Inventory.HasEmptyInventorySlot(data.Character.Level)).Execute(out var claimData, out string claimError))
                {
                    ClaimQuestRewards.Update(_account, claimData);
                    _account.Logger.Info($"Claimed rewards for mission");

                    result = RoutineResult.Finished;
                    error = "";
                    return true;
                }
                else
                {
                    _account.Logger.Warn($"Unable to claim rewards for mission: {claimError}");
                    result = RoutineResult.UnhandledError;
                    error = claimError;

                    switch (claimError)
                    {
                        case "errUserNotAuthorized":
                            result = RoutineResult.Unauthorized;
                            break;

                        case "errRequestMaintenance":
                            result = RoutineResult.Maintenance;
                            break;

                        case "errRequestOutdatedClientVersion":
                            result = RoutineResult.OutdatedVersion;
                            break;

                        case "errServerStatus400": // bad request
                        case "errServerStatus403": // forbidden - introduced by cloudflare
                        case "errServerStatus429": // too many requests
                        case "errRequestBlocked":
                            result = RoutineResult.Blocked;
                            break;
                    }

                    return false;
                }
            }

            result = RoutineResult.Sleeping;
            error = "";
            return true;
        }

        private int GetEnergyRefills()
        {
            var data = _account.HeroZero!.Data;

            // should use constants from game
            return (HeroZero.Constants.MissionEnergyRefillMax / HeroZero.Constants.MissionEnergyRefillAmount) - Math.Max(0, data.Character.MissionEnergyRefilled) / HeroZero.Constants.MissionEnergyRefillAmount;
        }

        private int GetTotalEnergy()
        {
            var data = _account.HeroZero!.Data;

            return GetEnergyRefills() * HeroZero.Constants.MissionEnergyRefillAmount + Math.Max(0, data.Character.MissionEnergy);
        }

        private Mission? GetActiveMission()
        {
            var data = _account.HeroZero!.Data;

            return data.Missions.Where(mission => mission.Id == data.Character.ActiveMissionId).FirstOrDefault();
        }

        private Mission GetFallbackMission()
        {
            var data = _account.HeroZero!.Data;
            var missions = data.Missions;

            var eventMissions = missions.Where(mission => mission.IsEventMission()).ToList();
            if (eventMissions.Any())
            {
                return eventMissions.OrderBy(mission => mission.EnergyCost).First();
            }

            return data.Missions.OrderBy(mission => mission.EnergyCost).First();
        }

        private Mission GetPreferredMission()
        {
            var data = _account.HeroZero!.Data;

            var missions = data.Missions.Where(mission => GetTotalEnergy() >= mission.EnergyCost && _config.MissionMaxEnergy >= mission.EnergyCost).ToList();

            if (!_config.MissionAllowTimed)
            {
                missions = missions.Where(mission => mission.IsFightMission()).ToList();
            }

            if (!missions.Any())
            {
                return GetFallbackMission();
            }

            Mission? bestEventMission = null;
            if (_config.PreferEventMissions)
            {
                var eventMissions = missions.Where(mission => mission.IsEventMission()).ToList();
                if (eventMissions.Any())
                {
                    if (_config.MissionPreferredSelect == ExecutionConfiguration.MissionSelect.GoldRatio)
                    {
                        return missions.OrderByDescending(mission => mission.Rewards.Gold / mission.EnergyCost).First();
                    }

                    if (_config.MissionPreferredSelect == ExecutionConfiguration.MissionSelect.ExpRatio)
                    {
                        return missions.OrderByDescending(mission => mission.Rewards.Experience / mission.EnergyCost).First();
                    }
                }
            }

            Mission? bestMission = null;
            double ratio = 0;
            if (_config.MissionPreferredSelect == ExecutionConfiguration.MissionSelect.GoldRatio)
            {
                bestMission = missions.OrderByDescending(mission => mission.Rewards.Gold / mission.EnergyCost).First();
                ratio = bestMission.Rewards.Gold / bestMission.EnergyCost;
            }

            if (_config.MissionPreferredSelect == ExecutionConfiguration.MissionSelect.ExpRatio)
            {
                bestMission = missions.OrderByDescending(mission => mission.Rewards.Experience / mission.EnergyCost).First();
                ratio = bestMission.Rewards.Experience / bestMission.EnergyCost;
            }

            if (ratio < _config.MissionMinimumRatio)
            {
                bestMission = GetFallbackMission();
            }

            return bestMission!;
        }

        private bool IsBetter(Mission? normal, Mission? eventMission)
        {
            if (normal == null) return false;
            if (eventMission == null) return true;

            if (_config.MissionPreferredSelect == ExecutionConfiguration.MissionSelect.GoldRatio)
            {
                return (normal.Rewards.Gold / normal.EnergyCost) >= (eventMission.Rewards.Gold / eventMission.EnergyCost);
            }

            return (normal.Rewards.Experience / normal.EnergyCost) >= (eventMission.Rewards.Experience / eventMission.EnergyCost);
        }

        private int GetEnergyRefillCost()
        {
            var data = _account.HeroZero!.Data;

            int factor = 0;
            switch (data.Character.MissionEnergyRefilled / HeroZero.Constants.MissionEnergyRefillAmount)
            {
                case 0:
                    factor = HeroZero.Constants.MissionEnergyRefillFactor1;
                    break;

                case 1:
                    factor = HeroZero.Constants.MissionEnergyRefillFactor2;
                    break;

                case 2:
                    factor = HeroZero.Constants.MissionEnergyRefillFactor3;
                    break;

                case 3:
                    factor = HeroZero.Constants.MissionEnergyRefillFactor4;
                    break;

                case 4:
                    factor = HeroZero.Constants.MissionEnergyRefillFactor5;
                    break;

                case 6:
                    factor = HeroZero.Constants.MissionEnergyRefillFactor7;
                    break;

                case 7:
                    factor = HeroZero.Constants.MissionEnergyRefillFactor8;
                    break;
            }

            return (int)Math.Round(factor * GetCurrencyPerTime(data.Character.Level), MidpointRounding.AwayFromZero);
        }

        private double GetCurrencyPerTime(int level)
        {
            return Math.Round(HeroZero.Constants.CoinBase + HeroZero.Constants.CoinScale * Math.Pow(HeroZero.Constants.CoinLevelScale * level, HeroZero.Constants.CoinLevelExperience), 3);
        }

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            return new MissionRoutine(account, config).Execute(out result, out error);
        }
    }
}
