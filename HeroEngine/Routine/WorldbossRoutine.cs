using HeroEngine.Framework;
using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Request.Mission;
using HeroEngine.Request.Villain;
using HeroEngine.Util;
using Newtonsoft.Json;
using System.Security.Claims;

namespace HeroEngine.Routine
{
    public class WorldbossRoutine : Routine<RoutineResult>
    {
        protected ExecutionConfiguration _config;
        public WorldbossRoutine(Account account, ExecutionConfiguration config) : base(account)
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

            if (!_config.WorldbossVillain)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            var data = _account.HeroZero!.Data;

            if (data.ActiveWorldBosses.Count <= 0)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            if (data.Character.ActiveMissionId != 0)
            {
                var activeMission = data.Missions.Where(mission => mission.Id == data.Character.ActiveMissionId).FirstOrDefault();
                if (activeMission != null)
                {
                    var remaining = TimeSpan.FromSeconds(UnixTime.Until(activeMission.TimeComplete));
                    if (remaining > TimeSpan.Zero)
                    {
                        if (new AbortQuest(_account).Execute(out var abortData, out string abortError))
                        {
                            AbortQuest.Update(_account, abortData);
                            _account.Logger.Info("Aborted active mission");
                        }
                        else
                        {
                            _account.Logger.Info($"Unable to abort active mission {data.Character.ActiveMissionId}, {abortError}");
                        }
                    } else
                    {
                        //finish
                        if (new CheckQuestComplete(_account).Execute(out var checkData, out string checkError))
                        {
                            CheckQuestComplete.Update(_account, checkData);
                            _account.Logger.Info($"Check if mission is complete (success, mission is complete)");
                        }

                        if (new ClaimQuestRewards(_account, !data.Inventory.HasEmptyInventorySlot(data.Character.Level)).Execute(out var claimData, out string claimError))
                        {
                            ClaimQuestRewards.Update(_account, claimData);
                            _account.Logger.Info($"Claimed rewards for mission");
                        }
                    }

                }
            }

            WorldBoss activeBoss = data.ActiveWorldBosses[0];
            if (data.Character.WorldbossId == 0)
            {
                if (!new AssignWorldbossEvent(_account, activeBoss.Id).Execute(out var assignData, out string assignError))
                {
                    _account.Logger.Warn($"Unable to assign worldboss event, {assignError}");
                    _account.Logger.Warn($"Character boss id: {data.Character.WorldbossId}");

                    result = RoutineResult.Finished;
                    error = "";
                    return true;
                }

                _account.Logger.Info("Assigned worldboss event");
                AssignWorldbossEvent.Update(_account, assignData);
            }

            if (data.ActiveWorldBossAttack != null)
            {
                if (data.ActiveWorldBossAttack.Status != 1)
                {
                    _account.Logger.Warn($"Weird worldboss status != 1: {data.ActiveWorldBossAttack.Status}");

                    result = RoutineResult.Finished;
                    error = "";
                    return true;
                }

                if (UnixTime.Since(data.ActiveWorldBossAttack.TimeComplete) > 0)
                {
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

                    if (new FinishWorldbossAttack(_account, activeBoss.Id).Execute(out var finishData, out string finishError))
                    {
                        _account.Logger.Info("finish worldboss attack");
                        FinishWorldbossAttack.Update(_account, finishData);
                        data.ActiveWorldBossAttack = null;
                    }
                    else
                    {
                        _account.Logger.Warn($"Unable to finish worldboss attack {activeBoss.Id}, {finishError}");
                    }
                } else
                {
                    result = RoutineResult.Finished;
                    error = "";
                    return true;
                }
            }

            if (!new StartWorldbossAttack(_account, activeBoss.Id).Execute(out var startData, out string startError))
            {
                //errStartWorldbossAttackActiveWorldbossAttackFound -> check complete
                // errStartWorldbossAttackInvalidWorldbossEvent -> end worldboss

                if (startError == "errStartWorldbossAttackActiveWorldbossAttackFound")
                {
                    if (new CheckForWorldbossAttackComplete(_account).Execute(out var checkData, out string checkError))
                    {
                        _account.Logger.Info("Check worldboss attack is complete");
                        CheckForWorldbossAttackComplete.Update(_account, checkData);
                    }
                    else
                    {
                       _account.Logger.Warn($"Unable to check worldboss attack complete, {checkError}");
                    }
                }

                if (startError == "errStartWorldbossAttackInvalidWorldbossEvent")
                {
                    if (!new ClaimWorldbossEventRewards(_account, activeBoss.Id).Execute(out var claimData, out string claimError))
                    {
                        result = RoutineResult.UnhandledError;
                        error = "";
                        return true;
                    }

                    _account.Logger.Info("Claim worldboss event rewards (finish boss)");
                    ClaimWorldbossEventRewards.Update(_account, claimData);
                    data.Character.ActiveBossAttackId = 0;
                    data.ActiveWorldBosses.Clear();
                    data.ActiveWorldBossAttack = null;

                    result = RoutineResult.Finished;
                    error = "";
                    return true;
                }

                if (startError == "errUserNotAuthorized")
                {
                    _account.Status = Account.AccountStatus.Unauthorized;
                    result = RoutineResult.Unauthorized;
                    error = "";
                    return true;
                }

                _account.Logger.Warn($"Unable to start worldboss attack, {startError}");
                _account.Logger.Warn($"Event id: {activeBoss.Id}");

                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            _account.Logger.Info($"Started world boss attack {activeBoss.Id}");
            StartWorldbossAttack.Update(_account, startData);

            WorldBossAttack attack = JsonConvert.DeserializeObject<WorldBossAttack>(JsonConvert.SerializeObject(startData.worldboss_attack));
            data.ActiveWorldBossAttack = attack;



            // data.Character.WorldbossId == 0, assign?

            //data.Character.ActiveBossAttackId;
            //activeBoss.Id;


            //check if active mission   
            //check if we can finish it



            //x.get_worldboss_event_multi_attack_min_hp=function(){return U.current.getNumber("worldboss_event_multi_attack_min_hp")}

            result = RoutineResult.Finished;
            error = "";
            return true;
        }

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            return new WorldbossRoutine(account, config).Execute(out result, out error);
        }
    }
}
