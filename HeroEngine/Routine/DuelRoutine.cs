using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Request;
using HeroEngine.Request.Duel;
using Newtonsoft.Json;

namespace HeroEngine.Routine
{
    public class DuelRoutine : Routine<RoutineResult>
    {
        protected ExecutionConfiguration _config;
        public DuelRoutine(Account account, ExecutionConfiguration config) : base(account)
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

            if (!_config.Duels)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            var data = _account.HeroZero!.Data;

            if (data.Character.DuelEnergy >= data.Character.DuelEnergyCost)
            {
                //also should take into account that we can have multiple throwables equipped!
                if (_config.DuelsUnequipThrowables && data.Inventory.HasEmptyInventorySlot(data.Character.Level) && data.Inventory.Throwable > 0)
                {
                    int firstEmptySlot = data.Inventory.FindEmptyInventorySlotIndex(data.Character.Level) + 9;
                    if (new MoveInventoryItem(_account, data.Inventory.Throwable, firstEmptySlot, MoveInventoryItem.InventoryActionType.Unequip).Execute(out var moveItemData, out string moveItemError))
                    {
                        MoveInventoryItem.Update(_account, moveItemData);
                        _account.Logger.Info("Unequipped throwables for duel");
                    }
                    else
                    {
                        _account.Logger.Warn($"Unable to unequip throwables for duel, {moveItemError}");
                    }
                }
            }
            
            //consider trophies for duels!
            //need a better calculation logic, because this will only go up if we sync game...
            while (data.Character.DuelEnergy >= data.Character.DuelEnergyCost)
            {
                if (data.Character.ActiveDuelId == 0)
                {
                    if (new GetDuelOponents(_account).Execute(out var opponentsData, out string opponentsError))
                    {
                        GetDuelOponents.Update(_account, opponentsData);

                        List<Opponent> opponents = JsonConvert.DeserializeObject<List<Opponent>>(JsonConvert.SerializeObject(opponentsData.opponents));

                        var opponent = GetOpponentWithLowestStats(opponents);
                        if (opponent == null)
                        {
                            //select opponent from leaderboard
                            result = RoutineResult.UnhandledError;
                            error = "there are no valid opponents left";
                            return false;
                        }

                        int opponentId = opponent.Id;

                        if (new StartDuel(_account, opponentId).Execute(out var startData, out string startError))
                        {
                            StartDuel.Update(_account, startData);

                            int winnerId = (startData.battle.winner ?? "a") == "a" ? startData.duel.character_a_id : startData.duel.character_b_id;
                            bool won = _account.HeroZero.Data.Character.Id == winnerId;

                            _account.Logger.Info($"{(won == true ? "won" : "lost")} duel against {opponent.Name}");

                        }
                        else
                        {
                            result = RoutineResult.UnhandledError;
                            error = startError;

                            //errStartDuelInvalidEnemy
                            //errStartDuelAttackCurrentlyNotAllowed
                            //errStartDuelActiveDuelFound

                            _account.Logger.Warn($"Unable to start duel: {startError}");

                            switch (opponentsError)
                            {
                                case "errStartDuelAttackCurrentlyNotAllowed":
                                    result = RoutineResult.Sleeping;
                                    return true;

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

                            return false;
                        }
                    }
                    else
                    {
                        result = RoutineResult.UnhandledError;
                        error = opponentsError;

                        _account.Logger.Warn($"Unable to get duel opponents: {opponentsError}");

                        switch (opponentsError)
                        {
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

                        return false;
                    }
                }

                if (new CheckForDuelComplete(_account).Execute(out var checkData, out string checkError))
                {
                    CheckForDuelComplete.Update(_account, checkData);
                    _account.Logger.Info($"Check if duel fight is complete (success, fight is complete)");
                }
                else
                {
                    result = RoutineResult.UnhandledError;
                    error = checkError;

                    _account.Logger.Warn($"Unable to check if duel fight is complete: {checkError}");

                    switch (checkError)
                    {
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

                if (new ClaimDuelRewards(_account, !data.Inventory.HasEmptyInventorySlot(data.Character.Level)).Execute(out var claimData, out string claimError))
                {
                    ClaimDuelRewards.Update(_account, claimData);
                    _account.Logger.Info($"Claimed rewards for duel fight");
                }
                else
                {
                    result = RoutineResult.UnhandledError;
                    error = claimError;

                    //errInventoryNoEmptySlot

                    _account.Logger.Warn($"Unable to claim rewards for duel fight: {claimError}");

                    switch (claimError)
                    {
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

                    return false;
                }
            }

            if (_config.DuelsUnequipThrowables && data.Inventory.Throwable == 0)
            {
                var throwablesInInventory = data.Items
                    .Where(item => item.Type == 8)
                    .Where(item => data.Inventory.Contains(data.Character.Level, item.Id));

                if (throwablesInInventory.Any())
                {
                    int throwableId = throwablesInInventory.First().Id;
                    if (new MoveInventoryItem(_account, throwableId, 8, MoveInventoryItem.InventoryActionType.Unequip).Execute(out var moveItemData, out string moveItemError))
                    {
                        MoveInventoryItem.Update(_account, moveItemData);
                        _account.Logger.Info("Equipped throwables after duel");
                    }
                    else
                    {
                        _account.Logger.Warn($"Unable to equip throwables after duel, {moveItemError}");
                    }
                }
            }

            result = RoutineResult.Finished;
            error = "";
            return true;
        }

        public Opponent? GetOpponentWithLowestStats(List<Opponent> opponents)
        {
            var validOpponents = opponents.Where(opponent => !opponent.Name.Contains("deleted"));

            /*var data = _account.HeroZero!.Data;
            if (data.GuildMembers != null && !_config.DuelsAttackTeamMembers)
            {
                validOpponents = validOpponents.Where(opponent => !data.GuildMembers.Any(member => member.Name == opponent.Name));
            }*/

            return validOpponents.OrderBy(opponent => opponent.Strength + opponent.Stamina + opponent.DodgeRating + opponent.CriticalRating).FirstOrDefault();
        }

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            return new DuelRoutine(account, config).Execute(out result, out error);
        }
    }
}
