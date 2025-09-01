using HeroEngine.Framework;
using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Request.Hideout;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Routine
{
    // sync game doesnt sync hideout!
    public class HideoutRoutine : Routine<RoutineResult>
    {
        protected ExecutionConfiguration _config;
        public HideoutRoutine(Account account, ExecutionConfiguration config) : base(account)
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

            var data = _account.HeroZero!.Data;

            if (data.Hideout == null || data.HideoutRooms == null || data.HideoutRooms.Count == 0)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            if (_config.HideoutAttacks)
            {
                if (data.Hideout.ActiveBattleId != 0)
                {
                    if (new CollectHideoutFightRewards(_account, data.Hideout.ActiveBattleId).Execute(out var rewardsData, out string rewardsError))
                    {
                        CollectHideoutFightRewards.Update(_account, rewardsData);
                        _account.Logger.Info("Collected hideout fight rewards");
                    }
                    else
                    {
                        _account.Logger.Warn($"Unable to collect hideout fight rewards: {rewardsError}");

                        result = RoutineResult.UnhandledError;
                        error = rewardsError;
                        return false;
                    }
                }

                foreach (var room in data.HideoutRooms.Where(r => !r.IsRoomUpgrading() && r.Status == 6 && r.IsAttackerProductionRoom() && r.MaxResources > 0 && UnixTime.Since(r.TimeActivityFinishes) > 5))
                {
                    int collected = room.MaxResources;
                    if (new CollectHideoutRoomActivityResult(_account, room.Id).Execute(out var collectData, out string collectError))
                    {
                        CollectHideoutRoomActivityResult.Update(_account, collectData);
                        _account.Logger.Info($"Collected {collected} attack bots from room {room.GetRoomName()} {room.Id}");
                    }
                    else
                    {
                        switch (collectError)
                        {
                            case "errUserNotAuthorized":
                                break;
                            default:
                                _account.Logger.Warn($"Unable to collect attack bots from room {room.Id}: {collectError}");

                                result = RoutineResult.UnhandledError;
                                error = collectError;
                                return false;
                        }
                    }
                }

                while (UnixTime.Since(data.Hideout.TimeLostAttack) > (HeroZero.Constants.HideoutLostFightCooldownMinutes * 60) && data.Hideout.Attackers > 0)
                {
                    if (new GetHideoutOpponent(_account).Execute(out var opponentData, out string opponentError))
                    {
                        GetHideoutOpponent.Update(_account, opponentData);

                        HideoutOpponent opponent = JsonConvert.DeserializeObject<HideoutOpponent>(JsonConvert.SerializeObject(opponentData.hideout_opponent.opponent));

                        int attackers = GetAttackersForAttack(opponent.WallLevel + 1, opponent.DefenderUnits, opponent.BarracksLevel, data.Hideout.BarracksLevel + 1, data.Hideout.Attackers);

                        int attackerNeeded = GetAttackersForSuccessfulAttack(opponent.WallLevel + 1, opponent.DefenderUnits, opponent.BarracksLevel, data.Hideout.BarracksLevel + 1, 5000);
                        if (attackerNeeded > data.Hideout.MaxAttackers && UnixTime.Since(data.Hideout.TimeOpponentRefreshed) > 600) // refresh opponent
                        {
                            if (new RefreshHideoutOpponent(_account, false).Execute(out var refreshData, out string refreshError))
                            {
                                RefreshHideoutOpponent.Update(_account, refreshError);

                                _account.Logger.Info($"Refreshed hideout opponent because previous opponent couldn't be beated, {attackerNeeded}+ attackers for win");

                                opponent = JsonConvert.DeserializeObject<HideoutOpponent>(JsonConvert.SerializeObject(refreshData.hideout_opponent.opponent));
                                attackers = GetAttackersForAttack(opponent.WallLevel + 1, opponent.DefenderUnits, opponent.BarracksLevel, data.Hideout.BarracksLevel + 1, data.Hideout.Attackers);
                            } else
                            {
                                _account.Logger.Warn($"Unable to refresh hideout opponent, {refreshError}");
                            }
                        }

                        
                        
                        int finalAttackers = attackers;
                        if (attackers > data.Hideout.Attackers || attackers > data.Hideout.MaxAttackers)
                        {
                            finalAttackers = 1;
                        }

                        int glue = opponent.Rewards.HideoutGlue + (opponent.BonusRewards != null ? opponent.BonusRewards.HideoutGlue : 0);
                        int stone = opponent.Rewards.HideoutZeronite + (opponent.BonusRewards != null ? opponent.BonusRewards.HideoutZeronite : 0);
                        if (new StartHideoutFight(_account, finalAttackers, glue, stone).Execute(out var startData, out string startError))
                        {
                            StartHideoutFight.Update(_account, startData);

                            bool won = startData.hideout_battle.attacker_hideout_id == startData.hideout_battle.hideout_winner_id;

                            _account.Logger.Info($"{(won == true ? "won" : "lost")} hideout fight against: {startData.hideout_battle.defender_character_name}, used {finalAttackers} bots");
                            if (!won) _account.Logger.Info($"Needed bots for likely hideout fight victory: {GetAttackersForSuccessfulAttack(opponent.WallLevel + 1, opponent.DefenderUnits, opponent.BarracksLevel, data.Hideout.BarracksLevel + 1, 5000)}+ (had {data.Hideout.Attackers} bots available)");

                            if (!won) GetRatingForAttack(opponent.WallLevel + 1, opponent.DefenderUnits, opponent.BarracksLevel, data.Hideout.BarracksLevel + 1, data.Hideout.Attackers);

                            if (new CollectHideoutFightRewards(_account, data.Hideout.ActiveBattleId).Execute(out var rewardsData, out string rewardsError))
                            {
                                CollectHideoutFightRewards.Update(_account, rewardsData);
                            }
                            else
                            {
                                //errCollectHideoutFightRewardsInvalidBattle
                                _account.Logger.Warn($"Unable to collect hideout fight rewards: {rewardsError}");

                                result = RoutineResult.UnhandledError;
                                error = rewardsError;
                                return false;
                            }
                        }
                        else
                        {
                            switch (startError)
                            {
                                case "errStartHideoutFightNotJetPossible":
                                case "errStartHideoutFightNotYetAllowed":
                                case "errStartHideoutFightRewardsChanged":
                                    break;

                                default:
                                    _account.Logger.Warn($"Unable to start hideout fight: {startError}");

                                    result = RoutineResult.UnhandledError;
                                    error = startError;
                                    return false;
                            }
                        }
                    }
                    else
                    {
                        _account.Logger.Warn($"Unable to get hideout opponent: {opponentError}");

                        result = RoutineResult.UnhandledError;
                        error = opponentError;
                        return false;
                    }
                }

                var storageRoom = GetHideoutRoom("robot_storage");
                int storageMultiplier = storageRoom != null ? storageRoom.Level + 1 : 1;

                var attackerRooms = data.HideoutRooms.Where(r => data.Hideout.IsRoomPlaced(r.Id) && !r.IsRoomUpgrading() && r.IsAttackerProductionRoom() && r.MaxResources <= 0).ToList();

                //this needs to be done on every loop

                foreach (var attackerRoom in attackerRooms)
                {
                    int maxAttackers = data.Hideout.MaxAttackers * storageMultiplier - data.Hideout.Attackers;
                    foreach (var attackerRoom2 in data.HideoutRooms.Where(r => data.Hideout.IsRoomPlaced(r.Id) && !r.IsRoomUpgrading() && r.IsAttackerProductionRoom() && r.MaxResources > 0))
                    {
                        maxAttackers -= attackerRoom2.MaxResources;
                    }

                    if (maxAttackers > 0)
                    {
                        int roomProductionLimit = Math.Min(attackerRoom.Level, maxAttackers);

                        if (roomProductionLimit > 0)
                        {
                            if (new StartHideoutRoomProduction(_account, attackerRoom.Id, roomProductionLimit).Execute(out var productionData, out string productionError))
                            {
                                StartHideoutRoomProduction.Update(_account, productionData);
                                _account.Logger.Info($"Started attack bot production in {attackerRoom.Id} (making {roomProductionLimit} attackers)");

                                if (data.Hideout.Attackers + roomProductionLimit >= data.Hideout.MaxAttackers * storageMultiplier) break;
                            }
                            else
                            {
                                _account.Logger.Warn($"Unable to start attack bot production in {attackerRoom.Id} {roomProductionLimit} attackers: {productionError}");

                                result = RoutineResult.UnhandledError;
                                error = productionError;
                                return false;
                            }
                        }
                    }
                }
                

                /*foreach (var room in data.HideoutRooms.Where(r => !r.IsRoomUpgrading() && r.IsAttackerProductionRoom()))
                {
                    int producingCount = 0;
                    foreach (var room2 in data.HideoutRooms.Where(r => !r.IsRoomUpgrading() && r.IsAttackerProductionRoom()))
                    {
                        producingCount += room2.MaxResources;
                    }

                    int productionLimit = (data.Hideout.MaxAttackers * storageMultiplier) - data.Hideout.Attackers;

                    if (room.MaxResources > 0)
                    {
                        continue;
                    }

                    //12:32:34] [PL34 HOWAN/WARNING]: Unable to start attack bot production in 20121 (7) room level (7 limit / 7 attackers): errStartHideoutRoomProductionInvalidProductionCount
                    int roomProductionLimit = Math.Min(room.Level, productionLimit);
                    if (roomProductionLimit > 0)
                    {
                        if (new StartHideoutRoomProduction(_account, room.Id, roomProductionLimit).Execute(out var productionData, out string productionError))
                        {
                            StartHideoutRoomProduction.Update(_account, productionData);
                            _account.Logger.Info($"Started attack bot production in {room.Id} (making {roomProductionLimit} attackers)");

                            if (data.Hideout.Attackers + roomProductionLimit >= data.Hideout.MaxAttackers * storageMultiplier) break;
                        }
                        else
                        {
                            _account.Logger.Warn($"Unable to start attack bot production in {room.Id} ({room.Level}) room level ({productionLimit} limit / {roomProductionLimit} attackers): {productionError}");

                            result = RoutineResult.UnhandledError;
                            error = productionError;
                            return false;
                        }
                    }
                }*/
            }

            if (_config.HideoutCollectResources)
            {
                foreach (var room in data.HideoutRooms.Where(r => data.Hideout.IsRoomPlaced(r.Id) && !r.IsRoomUpgrading() && r.IsAutomaticProductionRoom() && UnixTime.Since(r.TimeResourceChanged) >= 125))
                {
                    if ((room.Identifier == "glue_production") && data.Hideout.Glue >= data.Hideout.MaxGlue) continue;
                    if ((room.Identifier == "stone_production") && data.Hideout.Glue >= data.Hideout.MaxGlue) continue;

                    if (new CollectHideoutRoomActivityResult(_account, room.Id).Execute(out var collectData, out string collectError))
                    {
                        CollectHideoutRoomActivityResult.Update(_account, collectData);
                        _account.Logger.Info($"Collected {room.GetProducedResourceName()} from {room.GetRoomName()} {room.Id}");
                    }
                    else
                    {
                        if (collectError == "errUserNotAuthorized") continue;

                        //errCollectActivityResultInvalidAmount
                        _account.Logger.Warn($"Unable to collect resources from {room.GetRoomName()}, id {room.Id}: {collectError}");

                        if (collectError == "errCollectHideoutRoomActivityNotJetPossible")
                        {
                            continue;
                        }

                        result = RoutineResult.UnhandledError;
                        error = collectError;
                        return false;
                    }
                }
            }

            if (_config.HideoutCollectChests)
            {
                List<OpticalChangeChest> chests = JsonConvert.DeserializeObject<List<OpticalChangeChest>>(data.OpticalChanges.ChestsAvailable)!;

                List<int> chestIds = chests.Select(chest => (int)chest.Id).ToList();
                if (chestIds.Count > 0)
                {
                    if (new OpenOpticalChangeChests(_account, chestIds).Execute(out var openChestData, out string openChestError))
                    {
                        OpenOpticalChangeChests.Update(_account, openChestData);
                        _account.Logger.Info($"Opened {chestIds.Count} hideout chests");
                    }
                    else
                    {
                        _account.Logger.Warn($"Unable to open hideout chests, {openChestError}");
                    }
                }
            }

            result = RoutineResult.Finished;
            error = "";
            return true;
        }

        public HideoutRoom? GetHideoutRoom(string identifier)
        {
            var data = _account.HeroZero!.Data;

            return data.HideoutRooms?.Where(r => r.Identifier == identifier).FirstOrDefault();
        }

        public int GetAttackersForAttack(int wallLevel, int defenderUnits, int defenderLevel, int attackUnitsLevel, int maxAttackUnits)
        {
            for (int i = 1; i <= maxAttackUnits; i++)
            {
                int unitsToAttack = i;
                int unitsLevel = attackUnitsLevel;

                double a = unitsToAttack * (int)Math.Pow(unitsLevel, 2);
                double c;

                if (wallLevel > 8)
                {
                    c = Math.Pow(wallLevel, 3) + defenderUnits * Math.Pow(unitsLevel, 2);
                }
                else if (wallLevel > 6)
                {
                    c = Math.Pow(wallLevel, 2) + defenderUnits * Math.Pow(unitsLevel, 2);
                }
                else
                {
                    c = wallLevel + defenderUnits * Math.Pow(unitsLevel, 2);
                }

                a /= c;

                if (a >= 1) //win_chance_easy >
                {
                    return i;
                }
            }

            return 1;
        }

        public int GetAttackersForSuccessfulAttack(int wallLevel, int defenderUnits, int defenderLevel, int attackUnitsLevel, int maxIterationCount)
        {
            for (int i = 1; i <= maxIterationCount; i++)
            {
                int unitsToAttack = i;
                int unitsLevel = attackUnitsLevel;

                double a = unitsToAttack * (int)Math.Pow(unitsLevel, 2);
                double c;

                if (wallLevel > 8)
                {
                    c = Math.Pow(wallLevel, 3) + defenderUnits * Math.Pow(unitsLevel, 2);
                }
                else if (wallLevel > 6)
                {
                    c = Math.Pow(wallLevel, 2) + defenderUnits * Math.Pow(unitsLevel, 2);
                }
                else
                {
                    c = wallLevel + defenderUnits * Math.Pow(unitsLevel, 2);
                }

                a /= c;

                if (a >= 1) //win_chance_easy >
                {
                    return i;
                }
            }

            return maxIterationCount;
        }

        public double GetRatingForAttack(int wallLevel, int defenderUnits, int defenderLevel, int attackUnitsLevel, int maxAttackUnits)
        {
            for (int i = 1; i <= maxAttackUnits; i++)
            {
                int unitsToAttack = i;
                int unitsLevel = attackUnitsLevel;

                double a = unitsToAttack * (int)Math.Pow(unitsLevel, 2);
                double c;

                if (wallLevel > 8)
                {
                    c = Math.Pow(wallLevel, 3) + defenderUnits * Math.Pow(unitsLevel, 2);
                }
                else if (wallLevel > 6)
                {
                    c = Math.Pow(wallLevel, 2) + defenderUnits * Math.Pow(unitsLevel, 2);
                }
                else
                {
                    c = wallLevel + defenderUnits * Math.Pow(unitsLevel, 2);
                }

                a /= c;
            }

            return 1;
        }

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            return new HideoutRoutine(account, config).Execute(out result, out error);
        }
    }
}
