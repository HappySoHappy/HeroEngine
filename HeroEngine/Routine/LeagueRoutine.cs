using HeroEngine.Framework;
using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Request;
using HeroEngine.Request.League;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Routine
{
    public class LeagueRoutine : Routine<RoutineResult>
    {
        protected ExecutionConfiguration _config;
        public LeagueRoutine(Account account, ExecutionConfiguration config) : base(account)
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

            if (!_config.League)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            var data = _account.HeroZero!.Data;

            if (data.LeagueLocked || data.Character.Honor <= HeroZero.Constants.LeagueMinimumHonor || data.Character.LeagueGroupId <= 0)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            /*
             * ba.prototype.get_maxDailyLeagueFights = function() {
    var a =
        x.get_league_max_daily_league_fights();
    if (this.get_hasGuild() && null != this.get_guild())
        for (var b = this.get_guild().get_allArtifactIds(), c = 0, d = b.length; c < d;) {
            var e = c++;
            1 == Mt.fromId(b[e]).get_type() && (a += x.get_guild_artifact_duel_stamina_daily_league_fight())
        }
    return a
};
             */

            if (data.Goals.LeagueStarted.Value >= HeroZero.Constants.LeagueFightsPerDay)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            if (data.Character.ActiveLeagueFightId == 0)
            {
                if (data.Character.LeagueEnergy < data.Character.LeagueEnergyCost)
                {
                    result = RoutineResult.Sleeping;
                    error = "";
                    return true;
                }

                if (_config.LeagueUnequipThrowables && data.Inventory.HasEmptyInventorySlot(data.Character.Level) && data.Inventory.Throwable > 0)
                {
                    int firstEmptySlot = data.Inventory.FindEmptyInventorySlotIndex(data.Character.Level) + 9;
                    if (new MoveInventoryItem(_account, data.Inventory.Throwable, firstEmptySlot, MoveInventoryItem.InventoryActionType.Unequip).Execute(out var moveItemData, out string moveItemError))
                    {
                        MoveInventoryItem.Update(_account, moveItemData);
                        _account.Logger.Info("Unequipped throwables for league fight");
                    }
                    else
                    {
                        _account.Logger.Warn($"Unable to unequip throwables for league fight, {moveItemError}");
                    }
                }

                if (new GetLeagueOpponents(_account).Execute(out var opponentsData, out string opponentsError))
                {
                    GetLeagueOpponents.Update(_account, opponentsData);

                    List<LeagueOpponent> opps = JsonConvert.DeserializeObject<List<LeagueOpponent>>(JsonConvert.SerializeObject(opponentsData.league_opponents));

                    List<Opponent> opponents = new List<Opponent>();
                    foreach (var leagueOpponent in opps)
                    {
                        opponents.Add(leagueOpponent.Opponent);
                    }

                    _account.Logger.Info($"Got {opponents.Count} league opponents");

                    var opponent = GetOpponentWithLowestStats(opponents);
                    if (opponent == null)
                    {
                        result = RoutineResult.UnhandledError;
                        error = "opponent is null";
                        return false;
                    }

                    int opponentId = opponent.Id;

                    if (new StartLeagueFight(_account, opponentId).Execute(out var startData, out string startError))
                    {
                        StartLeagueFight.Update(_account, opponentId);

                        int winnerId = (startData.battle.winner ?? "a") == "a" ? startData.league_fight.character_a_id : startData.league_fight.character_b_id;
                        bool won = _account.HeroZero.Data.Character.Id == winnerId;

                        _account.Logger.Info($"{(won == true ? "won" : "lost")} league fight against {opponent.Name}");

                    } else
                    {
                        result = RoutineResult.UnhandledError;
                        error = startError;

                        _account.Logger.Warn($"Unable to start league fight: {startError}");
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
                } else
                {
                    result = RoutineResult.UnhandledError;
                    error = opponentsError;

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

                    _account.Logger.Warn($"Unable to get league opponents (group: {data.Character.LeagueGroupId}): {opponentsError}");
                    return false;
                }
            }

            if (new CheckForLeagueFightComplete(_account).Execute(out var checkData, out string checkError))
            {
                CheckForLeagueFightComplete.Update(_account, checkData);
                _account.Logger.Info($"Checking if league fight is complete (success, fight is complete)");
            } else
            {
                result = RoutineResult.UnhandledError;
                error = checkError;

                _account.Logger.Info($"Unable to checking if league fight is complete: {checkError}");
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

            if (new ClaimLeagueFightRewards(_account, !data.Inventory.HasEmptyInventorySlot(data.Character.Level)).Execute(out var claimData, out string claimError))
            {
                ClaimLeagueFightRewards.Update(_account, claimData);

                _account.Logger.Info($"Claimed rewards for league fight");
            } else
            {
                result = RoutineResult.UnhandledError;
                error = claimError;

                //errInventoryNoEmptySlot
                _account.Logger.Warn($"Unable to claim rewards for league fight: {claimError}");
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


            if (_config.LeagueUnequipThrowables && data.Inventory.Throwable == 0)
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
                        _account.Logger.Info("Equipped throwables after league fight");
                    }
                    else
                    {
                        _account.Logger.Warn($"Unable to equip throwables after league fight, {moveItemError}");
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
            if (data.GuildMembers != null && !_config.LeagueAttackTeamMembers)
            {
                validOpponents = validOpponents.Where(opponent => !data.GuildMembers.Any(member => member.Name == opponent.Name));
            }*/

            return opponents.OrderBy(opponent => opponent.Strength + opponent.Stamina + opponent.DodgeRating + opponent.CriticalRating).FirstOrDefault();
        }

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            return new LeagueRoutine(account, config).Execute(out result, out error);
        }
    }
}
