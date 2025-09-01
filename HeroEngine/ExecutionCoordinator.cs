using HeroEngine.Framework;
using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Request;
using HeroEngine.Request.Duel;
using HeroEngine.Request.Guild;
using HeroEngine.Request.League;
using HeroEngine.Routine;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine
{
    public class ExecutionCoordinator
    {
        public bool Starting = false;
        public bool Running = false;
        public ExecutionConfiguration Configuration;
        protected Dictionary<Account, long> _accounts = new Dictionary<Account, long>();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public ExecutionCoordinator(List<Account> accounts, ExecutionConfiguration config)
        {
            foreach (var account in accounts)
            {
                _accounts.Add(account, 0);
            }

            Configuration = config;
        }

        public async void Start()
        {
            if (Running)
            {
                return;
            }

            Starting = true;

            HeroZero.Initialize();

            FileLogger.Instance.Info("Initialized Hero Zero");

            foreach (var account in _accounts.Keys.Where(acc => acc.Enabled))
            {
                if (LoginRoutine.Execute(account, out var result, out string loginError))
                {
                    account.Status = Account.AccountStatus.Ready;
                    _accounts[account] = UnixTime.Now();

                    account.Logger.Info("Logged in");

                    ExecutionConfiguration config = account.Configuration != null ? account.Configuration : Configuration;
                    if (!account.InternalState!.IsLoginBonusFailed && config.ClaimLoginBonus && account.HeroZero?.Data.UnclaimedLoginRewards != null && account.HeroZero?.Data.UnclaimedLoginRewards.Status == 1)
                    {
                        if (!new ClaimDailyLoginBonus(account, account.HeroZero.Data.UnclaimedLoginRewards.Id, !account.HeroZero.Data.Inventory.HasEmptyInventorySlot(account.HeroZero.Data.Character.Level)).Execute(out var bonusData, out string bonusError))
                        {
                            account.Logger.Warn("daily bonus fail: " + bonusError);
                            account.InternalState!.IsLoginBonusFailed = true;
                        }

                        ClaimDailyLoginBonus.Update(account, bonusData);
                        account.Logger.Info($"Claimed login rewards for day {account.HeroZero.Data.Character.DailyBonusStreak}");
                    }

                    if (new GetStreamMessages(account, "v").Execute(out var voucherData, out string voucherError))
                    {
                        List<Voucher> vouchers = JsonConvert.DeserializeObject<List<Voucher>>(JsonConvert.SerializeObject(voucherData.user_vouchers));
                        account.HeroZero!.Data.Vouchers = vouchers;
                        account.Logger.Info($"There are {vouchers.Where(v => !v.IsHideoutVoucher()).Count()} unused coupons");
                    }

                    //account.HeroZero!.DumpStateToFile($"Login Account for {account.Name}");
                } else
                {
                    //if account is banned, send dump info with config to server
                    account.Logger.Info("Unable to log in: " + loginError);
                }

                await Task.Delay(TimeSpan.FromSeconds(0.5));
            }

            if (!_accounts.Keys.Where(acc => acc.Status is Account.AccountStatus.Ready).Any())
            {
                Starting = false;
                Running = false;

                FileLogger.Instance.Info("There are no accounts to run.");
                return;
            }

            Starting = false;
            Running = true;
            FileLogger.Instance.Info("Starting execution");

            long remainingTime = 600 - (UnixTime.SinceMidnight() - 60);
            if (remainingTime > 0) FileLogger.Instance.Info($"Execution will be delayed {UnixTime.Format(remainingTime)}");

            while (true)
            {
                try
                {
                    await Tick(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    _cancellationTokenSource?.Dispose();
                    _cancellationTokenSource = new CancellationTokenSource();
                    Running = false;
                    foreach (var account in _accounts.Keys)
                    {
                        account.Status = Account.AccountStatus.Unauthorized;
                        _accounts[account] = 0;
                    }
                    break;
                }
            }
        }

        public Task Tick(CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                if ((UnixTime.SinceMidnight() - 60) < 600) // stop right before midnight and stop until 10 minutes after midnight
                {
                    foreach (var account in _accounts.Keys.Where(acc => acc.Status == Account.AccountStatus.Active || acc.Status == Account.AccountStatus.Ready || acc.Status == Account.AccountStatus.Undetermined))
                    {
                        account.Status = Account.AccountStatus.Unauthorized;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                    return;
                }

                foreach (var account in _accounts.Keys.Where(acc => acc.Enabled && acc.Status == Account.AccountStatus.Unauthorized).ToList())
                {
                    if (cancellationToken.IsCancellationRequested) return;

                    ExecutionConfiguration config = account.Configuration != null ? account.Configuration : Configuration;

                    if (UnixTime.Since(_accounts[account]) < Configuration.ReloginDelay)
                    {
                        continue;
                    }

                    if (LoginRoutine.Execute(account, out var result, out string loginError))
                    {
                        account.Status = Account.AccountStatus.Ready;
                        _accounts[account] = UnixTime.Now();

                        account.Logger.Info("Relogged");
                        if (new GetStreamMessages(account, "v").Execute(out var voucherData, out string voucherError))
                        {
                            List<Voucher> vouchers = JsonConvert.DeserializeObject<List<Voucher>>(JsonConvert.SerializeObject(voucherData.user_vouchers));
                            account.HeroZero!.Data.Vouchers = vouchers;

                            account.Logger.Info($"There are {vouchers.Where(v => !v.IsHideoutVoucher()).Count()} unused coupons");
                        }
                    } else
                    {
                        if (result is RoutineResult.OutdatedVersion or RoutineResult.Maintenance)
                        {
                            account.Status = Account.AccountStatus.Unauthorized;
                            if (config.KillOnMaintenance)
                            {
                                Abort();
                                return;
                            }


                            HeroZero.Initialize();
                            _accounts[account] = UnixTime.Now();
                            return;
                        }

                        if (result is RoutineResult.Blocked)
                        {
                            _accounts[account] = UnixTime.Now() + 300;
                            continue;
                        }

                        if (loginError.StartsWith("errServerStatus"))
                        {
                            continue;
                        }

                        account.Status = Account.AccountStatus.Undetermined;
                        _accounts.Remove(account);
                        account.Logger.Warn($"Unable to relogin: {loginError}");
                        continue;
                    }
                }

                foreach (var account in _accounts.Keys.Where(acc => acc.Enabled && acc.Status is Account.AccountStatus.Ready or Account.AccountStatus.Active))
                {
                    if (cancellationToken.IsCancellationRequested) return;

                    ExecutionConfiguration config = account.Configuration != null ? account.Configuration : Configuration;

                    account.Status = Account.AccountStatus.Active;

                    // just relogin from time to time, to reset states
                    if (UnixTime.Since(_accounts[account]) > 3600)
                    {
                        account.Status = Account.AccountStatus.Unauthorized;
                        continue;
                    }

                    if (account.Status == Account.AccountStatus.Active && config.InventorySellPreferredSelect != ExecutionConfiguration.InventorySellSelect.None && !account.InternalState!.IsInventoryFailed)
                    {
                        if (!InventoryRoutine.Execute(account, config, out var result, out string error))
                        {
                            switch (error)
                            {
                                case "errServerStatus401":
                                case "errUserNotAuthorized":
                                case "errRequestMaintenance":
                                case "errRequestOutdatedClientVersion":
                                case "errServerStatus400": // bad request
                                case "errServerStatus403": // forbidden - introduced by cloudflare
                                case "errServerStatus429": // too many requests
                                case "errServerStatus504": // service unavailable
                                case "errRequestBlocked":
                                    account.Status = Account.AccountStatus.Unauthorized;
                                    _accounts[account] = UnixTime.Now();
                                    break;

                                default:
                                    account.Logger.Warn("Inventory fail: " + error);
                                    account.InternalState!.IsInventoryFailed = true;
                                    break;
                            }
                        }
                    }

                    if (cancellationToken.IsCancellationRequested) return;

                    if (account.Status == Account.AccountStatus.Active && !account.InternalState!.IsBoosterFailed &&
                    (config.QuestBoosterPreferredSelect != ExecutionConfiguration.QuestBoosterSelect.None
                    || config.StatBoosterPreferredSelect != ExecutionConfiguration.StatBoosterSelect.None
                    || config.WorkBoosterPreferredSelect != ExecutionConfiguration.WorkBoosterSelect.None
                    || config.LeagueBoosterPreferredSelect != ExecutionConfiguration.LeagueBoosterSelect.None))
                    {
                        if (!BoosterRoutine.Execute(account, config, out var result, out string error))
                        {
                            switch (error)
                            {
                                case "errServerStatus401":
                                case "errUserNotAuthorized":
                                case "errRequestMaintenance":
                                case "errRequestOutdatedClientVersion":
                                case "errServerStatus400": // bad request
                                case "errServerStatus403": // forbidden - introduced by cloudflare
                                case "errServerStatus429": // too many requests
                                case "errServerStatus504": // service unavailable
                                case "errRequestBlocked":
                                    account.Status = Account.AccountStatus.Unauthorized;
                                    _accounts[account] = UnixTime.Now();
                                    break;

                                default:
                                    account.Logger.Warn("Booster fail: " + error);
                                    account.InternalState!.IsBoosterFailed = true;
                                    break;
                            }
                        }
                    }

                    if (cancellationToken.IsCancellationRequested) return;

                    if (UnixTime.Since(account.HeroZero?.TsLastSync ?? UnixTime.Now()) > 65)
                    {
                        if (SynchronizeRoutine.Execute(account, out var syncResult, out string syncError))
                        {
                            if (!account.InternalState!.IsAcknowledgingAttacksFailed)
                            {
                                if (account.Status == Account.AccountStatus.Active && account.HeroZero!.Data.MissedDuelAttacks > 0)
                                {
                                    if (new GetMissedDuelsNew(account).Execute(out var missedDuelsData, out string missedDuelsError))
                                    {
                                        if (new ClaimMissedDuelsRewards(account).Execute(out var claimDuelsData, out string claimDuelsError))
                                        {
                                            account.Logger.Info($"Acknowledged {account.HeroZero!.Data.MissedDuelAttacks} missed duel fight(s)");
                                            ClaimMissedDuelsRewards.Update(account, claimDuelsData);
                                        }
                                        else
                                        {
                                            account.Logger.Warn($"Unable to view missed duel fights, {claimDuelsError}");
                                            account.InternalState!.IsAcknowledgingAttacksFailed = true;
                                        }
                                    }
                                    else
                                    {
                                        account.Logger.Warn($"Unable to get missed duel fights, {missedDuelsError}");
                                        account.InternalState!.IsAcknowledgingAttacksFailed = true;
                                    }
                                }

                                if (account.Status == Account.AccountStatus.Active && account.HeroZero!.Data.MissedLeagueAttacks > 0)
                                {
                                    if (new GetMissedLeagueFights(account).Execute(out var missedLeagueData, out string missedLeagueError))
                                    {
                                        if (new ClaimMissedLeagueFightsRewards(account).Execute(out var claimLeagueData, out string claimLeagueError))
                                        {
                                            account.Logger.Info($"Acknowledged {account.HeroZero!.Data.MissedLeagueAttacks} missed league fight(s)");
                                            ClaimMissedLeagueFightsRewards.Update(account, claimLeagueData);
                                        }
                                        else
                                        {
                                            account.Logger.Warn($"Unable to view missed league fights, {claimLeagueError}");
                                            account.InternalState!.IsAcknowledgingAttacksFailed = true;
                                        }

                                    }
                                    else
                                    {
                                        account.Logger.Warn($"Unable to get missed league fights, {missedLeagueError}");
                                        account.InternalState!.IsAcknowledgingAttacksFailed = true;
                                    }
                                }
                            }
                            

                            if (account.Status == Account.AccountStatus.Active && !account.InternalState!.IsLoginBonusFailed && config.ClaimLoginBonus
                            && account.HeroZero?.Data.UnclaimedLoginRewards != null && account.HeroZero?.Data.UnclaimedLoginRewards!.IsUnclaimed() == true)
                            {
                                if (!new ClaimDailyLoginBonus(account, account.HeroZero.Data.UnclaimedLoginRewards.Id, !account.HeroZero.Data.Inventory.HasEmptyInventorySlot(account.HeroZero.Data.Character.Level)).Execute(out var bonusData, out string bonusError))
                                {
                                    switch (bonusError)
                                    {
                                        case "errServerStatus401":
                                        case "errUserNotAuthorized":
                                        case "errRequestMaintenance":
                                        case "errRequestOutdatedClientVersion":
                                        case "errServerStatus400": // bad request
                                        case "errServerStatus403": // forbidden - introduced by cloudflare
                                        case "errServerStatus429": // too many requests
                                        case "errServerStatus504": // service unavailable
                                        case "errRequestBlocked":
                                            account.Status = Account.AccountStatus.Unauthorized;
                                            _accounts[account] = UnixTime.Now();
                                            break;

                                        default:
                                            account.Logger.Warn("daily bonus fail: " + bonusError);
                                            account.InternalState!.IsLoginBonusFailed = true;
                                            break;
                                    }
                                }

                                ClaimDailyLoginBonus.Update(account, bonusData);
                                account.Logger.Info($"Claimed login rewards for day {account.HeroZero.Data.Character.DailyBonusStreak}");
                            }

                            if (account.Status == Account.AccountStatus.Active && !account.InternalState!.IsBatteryFailed && config.AcceptBatteryRequests)
                            {
                                if (!BatteryRequestRoutine.Execute(account, config, out var batteryResult, out string batteryError))
                                {
                                    account.Logger.Warn("send battery fail: " + batteryError);
                                    account.InternalState!.IsBatteryFailed = true;
                                }
                            }

                            if (account.Status == Account.AccountStatus.Active && !account.InternalState!.IsVoucherFailed)
                            {
                                if (new GetStreamMessages(account, "v").Execute(out var voucherData, out string voucherError))
                                {
                                    List<Voucher> vouchers = JsonConvert.DeserializeObject<List<Voucher>>(JsonConvert.SerializeObject(voucherData.user_vouchers));
                                    account.HeroZero!.Data.Vouchers = vouchers;
                                    account.Logger.Info($"There are {vouchers.Where(v => !v.IsHideoutVoucher()).Count()} unused coupons");
                                } else
                                {
                                    account.Logger.Warn($"Unable to get vouchers, {voucherError}");
                                    account.InternalState!.IsVoucherFailed = true;
                                }
                            }

                            if (account.Status == Account.AccountStatus.Active && config.RedeemVouchersLater && !account.InternalState!.IsVoucherFailed) // replace with config value
                            {
                                foreach (int voucherId in account.HeroZero!.Data.Character.UnusedVoucherIds)
                                {
                                    if (new GetUserVoucher(account, voucherId).Execute(out var userVoucherData, out string userVoucherError))
                                    {
                                        Voucher voucher = JsonConvert.DeserializeObject<Voucher>(JsonConvert.SerializeObject(userVoucherData.voucher));

                                        if (new RedeemUserVoucherLater(account, voucher.Code).Execute(out var redeemLaterData, out string redeemLaterError))
                                        {
                                            RedeemUserVoucherLater.Update(account, redeemLaterData);
                                            account.Logger.Info($"Redeemed voucher {voucher.Code} for later use");
                                        }
                                        else
                                        {
                                            account.Logger.Warn($"Unable to redeem voucher {voucher.Code} for later use, {redeemLaterError}");
                                            account.InternalState!.IsVoucherFailed = true;
                                        }
                                    }
                                    else
                                    {
                                        account.Logger.Warn($"Unable to get voucher {voucherId}, {userVoucherError}");
                                        account.InternalState!.IsVoucherFailed = true;
                                    }
                                }
                            }

                            if (account.Status == Account.AccountStatus.Active && !account.InternalState!.IsTrainingFailed && config.Training)
                            {
                                if (!TrainingRoutine.Execute(account, config, out var result, out string error))
                                {
                                    switch (error)
                                    {
                                        case "errServerStatus401":
                                        case "errUserNotAuthorized":
                                        case "errRequestMaintenance":
                                        case "errRequestOutdatedClientVersion":
                                        case "errServerStatus400": // bad request
                                        case "errServerStatus403": // forbidden - introduced by cloudflare
                                        case "errServerStatus429": // too many requests
                                        case "errServerStatus504": // service unavailable
                                        case "errRequestBlocked":
                                            account.Status = Account.AccountStatus.Unauthorized;
                                            _accounts[account] = UnixTime.Now();
                                            break;

                                        default:
                                            account.Logger.Warn("training fail: " + error);
                                            account.InternalState!.IsTrainingFailed = true;
                                            break;
                                    }
                                }
                            }
                        } else
                        {
                            switch (syncError)
                            {
                                case "errServerStatus401":
                                case "errUserNotAuthorized":
                                case "errRequestMaintenance":
                                case "errRequestOutdatedClientVersion":
                                case "errServerStatus400": // bad request
                                case "errServerStatus403": // forbidden - introduced by cloudflare
                                case "errServerStatus429": // too many requests
                                case "errServerStatus504": // service unavailable
                                case "errRequestBlocked":
                                    account.Status = Account.AccountStatus.Unauthorized;
                                    _accounts[account] = UnixTime.Now();
                                    break;

                                default:
                                    account.Logger.Warn("sync fail: " + syncError);
                                    account.InternalState!.IsTrainingFailed = true;
                                    break;
                            }
                        }
                    }

                    if (UnixTime.Since(account.HeroZero?.TsLastGuildSync ?? UnixTime.Now()) > 305 && account.HeroZero!.Data.Character.GuildId > 0 && !account.InternalState!.IsGuildFailed)
                    {
                        //claim rewards even if guild join on cooldown
                        if (new SyncGuild(account).Execute(out var guildSyncData, out string guildSyncError))
                        {
                            SyncGuild.Update(account, guildSyncData);
                            account.HeroZero!.TsLastGuildSync = UnixTime.Now();

                            account.Logger.Info("Synchronized guild");

                            var member = account.HeroZero.Data.GuildMembers?.Find(member => member.UserId == account.HeroZero.Data.User.Id);
                            if (member != null && UnixTime.Since(member.TimeGuildJoined) > 86400) // i think its full day cooldown?
                            {
                                //errAddCharacterNoPermission
                                if (account.Status == Account.AccountStatus.Active && account.HeroZero!.Data.Guild != null)
                                {
                                    if (account.HeroZero.Data.Character.FinishedGuildAttackId != 0 && config.JoinClaimGuildFights)
                                    {
                                        if (new ClaimGuildBattleReward(account, account.HeroZero.Data.Character.FinishedGuildAttackId, account.HeroZero.Data.Inventory.HasEmptyInventorySlot(account.HeroZero.Data.Character.Level))
                                        .Execute(out var claimAttackData, out string claimAttackError))
                                        {
                                            ClaimGuildBattleReward.Update(account, claimAttackData);
                                            account.Logger.Info("Claimed rewards for finished guild attack");
                                        }
                                        else
                                        {
                                            account.Logger.Warn($"Unable to claim rewards for finished guild attack, {claimAttackError}");
                                            account.InternalState!.IsGuildFailed = true;
                                        }
                                    }

                                    if (account.HeroZero.Data.Character.FinishedGuildDefenseId != 0 && config.JoinClaimGuildDefenses)
                                    {
                                        if (new ClaimGuildBattleReward(account, account.HeroZero.Data.Character.FinishedGuildDefenseId, account.HeroZero.Data.Inventory.HasEmptyInventorySlot(account.HeroZero.Data.Character.Level))
                                        .Execute(out var claimDefenseData, out string claimDefenseError))
                                        {
                                            ClaimGuildBattleReward.Update(account, claimDefenseData);
                                            account.Logger.Info("Claimed rewards for finished guild defense");
                                        }
                                        else
                                        {
                                            account.Logger.Warn($"Unable to claim rewards for finished guild defense, {claimDefenseError}");
                                            account.InternalState!.IsGuildFailed = true;
                                        }
                                    }

                                    if (account.HeroZero.Data.Character.FinishedGuildDungeonId != 0 && config.JoinClaimGuildDungeons)
                                    {
                                        if (new ClaimGuildDungeonBattleReward(account, account.HeroZero.Data.Character.FinishedGuildDungeonId, account.HeroZero.Data.Inventory.HasEmptyInventorySlot(account.HeroZero.Data.Character.Level))
                                        .Execute(out var claimAttackData, out string claimAttackError))
                                        {
                                            ClaimGuildDungeonBattleReward.Update(account, claimAttackData);
                                            account.Logger.Info("Claimed rewards for finished guild band attack");
                                        }
                                        else
                                        {
                                            //errInventoryNoEmptySlot
                                            account.Logger.Warn($"Unable to claim rewards for finished guild band attack, {claimAttackError}");
                                            account.InternalState!.IsGuildFailed = true;
                                        }
                                    }


                                    //errJoinGuildBattleInvalidGuildBattle
                                    if (account.HeroZero.Data.PendingGuildAttack != null
                                    && !account.HeroZero.Data.PendingGuildAttack.IsParticipating(account.HeroZero.Data.Character.Id) && config.JoinClaimGuildFights)
                                    {
                                        //attack=true&action=joinGuildBattle&user_id=3093&user_session_id=ZpoOLFHqg1VjxbVm7D18knBzQw12F8&client_version=html5_235&build_number=129&auth=efa73bad81d84287d000d1eb57df9dd8&rct=2&keep_active=true&device_type=web
                                        // -> {"data":{"user":{"id":3093,"premium_currency":447,"blocked_premium_currency":0},"character":{"id":3067,"stat_total_stamina":14415,"stat_total_strength":4392,"stat_total_critical_rating":14239,"stat_total_dodge_rating":14157,"active_quest_booster_id":"booster_quest3","active_stats_booster_id":"booster_stats3","active_work_booster_id":"booster_work2","ts_active_sense_boost_expires":0,"active_league_booster_id":"booster_league1","ts_active_league_boost_expires":1736063515,"ts_active_multitasking_boost_expires":-1,"ts_active_training_sense_boost_expires":0,"quest_energy":1,"max_quest_energy":120,"duel_stamina":21,"max_duel_stamina":100,"ts_last_duel_stamina_change":1735571025,"duel_stamina_cost":20,"training_energy":30,"max_training_energy":30,"ts_last_training_energy_change":1735563472,"training_count":0,"max_training_count":32,"active_worldboss_attack_id":0,"active_dungeon_quest_id":0,"guild_id":1,"guild_rank":3,"finished_guild_battle_attack_id":0,"finished_guild_battle_defense_id":0,"finished_guild_dungeon_battle_id":12851,"guild_donated_game_currency":500,"guild_donated_premium_currency":195,"guild_competition_points_gathered":99,"pending_guild_competition_tournament_rewards":0,"pending_solo_guild_competition_tournament_rewards":0,"worldboss_event_id":0,"worldboss_event_attack_count":0,"pending_tournament_rewards":0,"pending_global_tournament_rewards":0,"ts_last_shop_refresh":1735549571,"event_quest_id":0,"hidden_object_event_quest_id":1909,"draw_event_id":24910,"treasure_event_id":0,"legendary_dungeon_id":0,"league_group_id":1300001,"active_league_fight_id":0,"league_fight_count":10,"league_opponents":"[2857,4676,8946]","ts_last_league_opponents_refresh":0,"league_stamina":17,"max_league_stamina":60,"ts_last_league_stamina_change":1735571085,"league_stamina_cost":20,"pending_league_tournament_rewards":0,"slotmachine_spin_count":0,"new_user_voucher_ids":"[]"},"pending_guild_battle_attack":{"id":14182,"battle_time":1,"ts_attack":1735642800,"guild_attacker_id":1,"guild_defender_id":81,"status":1,"character_ids":"[1296,3403,1786,1339,3067]"},"time_correction":0.02571892738342285,"server_time":1735571179},"error":""}

                                        if (new JoinGuildBattle(account, true).Execute(out var joinAttackData, out string joinAttackError))
                                        {
                                            JoinGuildBattle.Update(account, joinAttackData);
                                            account.HeroZero.Data.PendingGuildAttack.ParticipatingCharacters.Append(account.HeroZero.Data.Character.Id);
                                            account.Logger.Info("Joined guild attack");
                                        }
                                        else
                                        {
                                            switch (joinAttackError)
                                            {
                                                case "errAddCharacterIdAlreadyJoined":
                                                    account.HeroZero.Data.PendingGuildAttack.ParticipatingCharacters.Append(account.HeroZero.Data.Character.Id);
                                                    break;

                                                default:
                                                    account.Logger.Warn($"Unable to join guild attack, {joinAttackError}");
                                                    account.InternalState!.IsGuildFailed = true;
                                                    break;
                                            }
                                        }
                                    }

                                    if (account.HeroZero.Data.PendingGuildDefense != null
                                    && !account.HeroZero.Data.PendingGuildDefense.IsParticipating(account.HeroZero.Data.Character.Id) && config.JoinClaimGuildDefenses)
                                    {
                                        //attack=false&action=joinGuildBattle&user_id=3093&user_session_id=ZpoOLFHqg1VjxbVm7D18knBzQw12F8&client_version=html5_235&build_number=129&auth=efa73bad81d84287d000d1eb57df9dd8&rct=2&keep_active=true&device_type=web
                                        // -> {"data":{"user":{"id":3093,"premium_currency":447,"blocked_premium_currency":0},"character":{"id":3067,"stat_total_stamina":14415,"stat_total_strength":4392,"stat_total_critical_rating":14239,"stat_total_dodge_rating":14157,"active_quest_booster_id":"booster_quest3","active_stats_booster_id":"booster_stats3","active_work_booster_id":"booster_work2","ts_active_sense_boost_expires":0,"active_league_booster_id":"booster_league1","ts_active_league_boost_expires":1736063515,"ts_active_multitasking_boost_expires":-1,"ts_active_training_sense_boost_expires":0,"quest_energy":1,"max_quest_energy":120,"duel_stamina":22,"max_duel_stamina":100,"ts_last_duel_stamina_change":1735571205,"duel_stamina_cost":20,"training_energy":30,"max_training_energy":30,"ts_last_training_energy_change":1735563472,"training_count":0,"max_training_count":32,"active_worldboss_attack_id":0,"active_dungeon_quest_id":0,"guild_id":1,"guild_rank":3,"finished_guild_battle_attack_id":0,"finished_guild_battle_defense_id":0,"finished_guild_dungeon_battle_id":12851,"guild_donated_game_currency":500,"guild_donated_premium_currency":195,"guild_competition_points_gathered":99,"pending_guild_competition_tournament_rewards":0,"pending_solo_guild_competition_tournament_rewards":0,"worldboss_event_id":0,"worldboss_event_attack_count":0,"pending_tournament_rewards":0,"pending_global_tournament_rewards":0,"ts_last_shop_refresh":1735549571,"event_quest_id":0,"hidden_object_event_quest_id":1909,"draw_event_id":24910,"treasure_event_id":0,"legendary_dungeon_id":0,"league_group_id":1300001,"active_league_fight_id":0,"league_fight_count":10,"league_opponents":"[2857,4676,8946]","ts_last_league_opponents_refresh":0,"league_stamina":18,"max_league_stamina":60,"ts_last_league_stamina_change":1735571220,"league_stamina_cost":20,"pending_league_tournament_rewards":0,"slotmachine_spin_count":0,"new_user_voucher_ids":"[]"},"pending_guild_battle_attack":{"id":14113,"battle_time":5,"ts_attack":1735592400,"guild_attacker_id":96,"guild_defender_id":1,"status":1,"character_ids":"[3558,269,1719,8558,2457,551,434,2979,692,439,2723,276,2213,3381,365,6197,1060]"},"time_correction":0.032628774642944336,"server_time":1735571221},"error":""}

                                        if (new JoinGuildBattle(account, false).Execute(out var joinDefenseData, out string joinDefenseError))
                                        {
                                            JoinGuildBattle.Update(account, joinDefenseData);
                                            account.HeroZero.Data.PendingGuildDefense.ParticipatingCharacters.Append(account.HeroZero.Data.Character.Id);
                                            account.Logger.Info("Joined guild defense");
                                        }
                                        else
                                        {
                                            account.Logger.Warn($"Unable to join guild defense, {joinDefenseError}");
                                            account.InternalState!.IsGuildFailed = true;
                                        }
                                    }

                                    if (account.HeroZero.Data.PendingGuildDungeon != null
                                    && !account.HeroZero.Data.PendingGuildDungeon.IsParticipating(account.HeroZero.Data.Character.Id) && config.JoinClaimGuildDungeons)
                                    {
                                        //action=joinGuildDungeonBattle&user_id=3093&user_session_id=ZpoOLFHqg1VjxbVm7D18knBzQw12F8&client_version=html5_235&build_number=129&auth=cf0e01e85cebf61b3ceb9d67caf54a01&rct=2&keep_active=true&device_type=web
                                        // -> {"data":{"user":{"id":3093,"premium_currency":447,"blocked_premium_currency":0},"character":{"id":3067,"stat_total_stamina":14415,"stat_total_strength":4392,"stat_total_critical_rating":14239,"stat_total_dodge_rating":14157,"active_quest_booster_id":"booster_quest3","active_stats_booster_id":"booster_stats3","active_work_booster_id":"booster_work2","ts_active_sense_boost_expires":0,"active_league_booster_id":"booster_league1","ts_active_league_boost_expires":1736063515,"ts_active_multitasking_boost_expires":-1,"ts_active_training_sense_boost_expires":0,"quest_energy":1,"max_quest_energy":120,"duel_stamina":22,"max_duel_stamina":100,"ts_last_duel_stamina_change":1735571205,"duel_stamina_cost":20,"training_energy":30,"max_training_energy":30,"ts_last_training_energy_change":1735563472,"training_count":0,"max_training_count":32,"active_worldboss_attack_id":0,"active_dungeon_quest_id":0,"guild_id":1,"guild_rank":3,"finished_guild_battle_attack_id":0,"finished_guild_battle_defense_id":0,"finished_guild_dungeon_battle_id":0,"guild_donated_game_currency":500,"guild_donated_premium_currency":195,"guild_competition_points_gathered":99,"pending_guild_competition_tournament_rewards":0,"pending_solo_guild_competition_tournament_rewards":0,"worldboss_event_id":0,"worldboss_event_attack_count":0,"pending_tournament_rewards":0,"pending_global_tournament_rewards":0,"ts_last_shop_refresh":1735549571,"event_quest_id":0,"hidden_object_event_quest_id":1909,"draw_event_id":24910,"treasure_event_id":0,"legendary_dungeon_id":0,"league_group_id":1300001,"active_league_fight_id":0,"league_fight_count":10,"league_opponents":"[2857,4676,8946]","ts_last_league_opponents_refresh":0,"league_stamina":19,"max_league_stamina":60,"ts_last_league_stamina_change":1735571355,"league_stamina_cost":20,"pending_league_tournament_rewards":0,"slotmachine_spin_count":0,"new_user_voucher_ids":"[]"},"pending_guild_dungeon_battle":{"id":12953,"battle_time":1,"ts_attack":1735642800,"guild_id":1,"npc_team_identifier":"team22","npc_team_character_profiles":"","status":2,"settings":"{\"difficulty\":2,\"rewards\":{\"1\":1.2,\"2\":1.2,\"3\":0,\"4\":0,\"5\":0,\"6\":0,\"7\":4,\"8\":0},\"reroll_count\":1}","character_ids":"[1786,2238,474,950,4676,1978,2861,2791,1296,13849,5031,3462,473,1078,7200,1339,3229,3403,359,1364,4970,4803,1536,1010,398,1031,3067]"},"time_correction":0.019153118133544922,"server_time":1735571366},"error":""}

                                        if (new JoinGuildDungeonBattle(account).Execute(out var joinDungeonData, out string joinDungeonError))
                                        {
                                            JoinGuildDungeonBattle.Update(account, joinDungeonData);
                                            account.HeroZero.Data.PendingGuildDungeon.ParticipatingCharacters.Append(account.HeroZero.Data.Character.Id);
                                            account.Logger.Info("Joined guild dungeon");
                                        }
                                        else
                                        {
                                            account.Logger.Warn($"Unable to join guild dungeon, {joinDungeonError}");
                                            account.InternalState!.IsGuildFailed = true;
                                        }
                                    }
                                }
                            }
                        } else
                        {
                            if (guildSyncError == "errUserNotAuthorized")
                            {
                                account.Status = Account.AccountStatus.Unauthorized;
                                continue;
                            }

                            account.Logger.Warn($"Unable to sync guild, {guildSyncError}");
                            account.InternalState!.IsGuildFailed = true;
                        }
                    }

                    //sync hideout every 7 minutes

                    if (cancellationToken.IsCancellationRequested) return;

                    if (account.Status == Account.AccountStatus.Active && !account.InternalState!.IsDuelsFailed && config.Duels)
                    {
                        //errStartDuelInvalidEnemy
                        if (!DuelRoutine.Execute(account, config, out var result, out string error))
                        {
                            switch (error)
                            {
                                case "errServerStatus401":
                                case "errUserNotAuthorized":
                                case "errRequestMaintenance":
                                case "errRequestOutdatedClientVersion":
                                case "errServerStatus400": // bad request
                                case "errServerStatus403": // forbidden - introduced by cloudflare
                                case "errServerStatus429": // too many requests
                                case "errServerStatus504": // service unavailable
                                case "errRequestBlocked":
                                    account.Status = Account.AccountStatus.Unauthorized;
                                    _accounts[account] = UnixTime.Now();
                                    break;

                                default:
                                    account.Logger.Warn("Duels fail: " + error);
                                    account.InternalState!.IsDuelsFailed = true;
                                    break;
                            }
                        }
                    }

                    if (cancellationToken.IsCancellationRequested) return;

                    if (account.Status == Account.AccountStatus.Active && !account.InternalState!.IsLeagueFailed && config.League)
                    {
                        if (!LeagueRoutine.Execute(account, config, out var result, out string error))
                        {
                            switch (error)
                            {
                                case "errServerStatus401":
                                case "errUserNotAuthorized":
                                case "errRequestMaintenance":
                                case "errRequestOutdatedClientVersion":
                                case "errServerStatus400": // bad request
                                case "errServerStatus403": // forbidden - introduced by cloudflare
                                case "errServerStatus429": // too many requests
                                case "errServerStatus504": // service unavailable
                                case "errRequestBlocked":
                                    account.Status = Account.AccountStatus.Unauthorized;
                                    _accounts[account] = UnixTime.Now();
                                    break;

                                default:
                                    account.Logger.Warn("league fail: " + error);
                                    account.InternalState!.IsLeagueFailed = true;
                                    break;
                            }
                        }
                    }

                    if (cancellationToken.IsCancellationRequested) return;

                    if (account.Status == Account.AccountStatus.Active && !account.InternalState!.IsDungeonsFailed && config.Dungeons)
                    {
                        if (!SpecialDungeonRoutine.Execute(account, config, out var result, out string error))
                        {
                            switch (error)
                            {
                                case "errServerStatus401":
                                case "errUserNotAuthorized":
                                case "errRequestMaintenance":
                                case "errRequestOutdatedClientVersion":
                                case "errServerStatus400": // bad request
                                case "errServerStatus403": // forbidden - introduced by cloudflare
                                case "errServerStatus429": // too many requests
                                case "errServerStatus504": // service unavailable
                                case "errRequestBlocked":
                                    account.Status = Account.AccountStatus.Unauthorized;
                                    _accounts[account] = UnixTime.Now();
                                    break;

                                default:
                                    account.Logger.Warn("Dungeons fail: " + error);
                                    account.InternalState!.IsDungeonsFailed = true;
                                    break;
                            }
                        }
                    }

                    if (cancellationToken.IsCancellationRequested) return;

                    if (account.Status == Account.AccountStatus.Active && !account.InternalState!.IsWorldbossFailed && config.WorldbossVillain)
                    {
                        if (!WorldbossRoutine.Execute(account, config, out var result, out string error))
                        {
                            account.Logger.Warn("WORLDBOSS ERROR: "+error);
                        }
                    }

                    if (account.Status == Account.AccountStatus.Active && !account.InternalState!.IsMissionFailed && config.Missions) // if running world boss and active multitasking run
                    {
                        /*if (account.HeroZero!.Data.ActiveWorldBosses.Count() == 0 || config.WorldbossVillain)
                        {
                            // errStartQuestActiveWorldbossAttackFound
                        }*/

                        if (!MissionRoutine.Execute(account, config, out var result, out string error))
                        {
                            switch (error)
                            {
                                case "errServerStatus401":
                                case "errUserNotAuthorized":
                                case "errRequestMaintenance":
                                case "errRequestOutdatedClientVersion":
                                case "errServerStatus400": // bad request
                                case "errServerStatus403": // forbidden - introduced by cloudflare
                                case "errServerStatus429": // too many requests
                                case "errServerStatus504": // service unavailable
                                case "errRequestBlocked":
                                    account.Status = Account.AccountStatus.Unauthorized;
                                    _accounts[account] = UnixTime.Now();
                                    break;

                                default:
                                    //Unable to check if mission is complete: One or more errors occurred. (Nieznany host. (pl34.herozerogame.com:443))
                                    account.Logger.Warn("Mission fail: " + error);
                                    account.InternalState!.IsMissionFailed = true;
                                    break;
                            }
                        }
                    }

                    if (cancellationToken.IsCancellationRequested) return;

                    if (account.Status == Account.AccountStatus.Active && !account.InternalState!.IsHideoutFailed && (config.HideoutAttacks || config.HideoutCollectResources || config.HideoutCollectChests))
                    {
                        if (!HideoutRoutine.Execute(account, config, out var result, out string error))
                        {
                            switch (error)
                            {
                                case "errServerStatus401":
                                case "errUserNotAuthorized":
                                case "errRequestMaintenance":
                                case "errRequestOutdatedClientVersion":
                                case "errServerStatus400": // bad request
                                case "errServerStatus403": // forbidden - introduced by cloudflare
                                case "errServerStatus429": // too many requests
                                case "errServerStatus504": // service unavailable
                                case "errRequestBlocked":
                                    account.Status = Account.AccountStatus.Unauthorized;
                                    _accounts[account] = UnixTime.Now();
                                    break;

                                default:

                                //[Thread10 @ 02:00:25] [PL34 OPOROWIEC/WARN]: Unable to get hideout opponent: errServerStatus500
                                //[Thread10 @ 02:00:25] [PL34 OPOROWIEC / WARN]: Hideout fail: errServerStatus500
                                    account.Logger.Warn("Hideout fail: " + error);
                                    account.InternalState!.IsHideoutFailed = true;
                                    break;
                            }
                        }
                    }

                    if (cancellationToken.IsCancellationRequested) return;

                    if (account.Status == Account.AccountStatus.Active && !account.InternalState!.IsTreasureFailed && config.Treasure)
                    {
                    //[Thread25 @ 00:00:14] [PL34 OPOROWIEC/WARN]: Unable to collect shovels: errClaimFreeTreasureRevealItemsInvalidEventId
                    //[Thread25 @ 00:00:14] [PL34 OPOROWIEC / WARN]: Treasure fail: errClaimFreeTreasureRevealItemsInvalidEventId
                        if (!TreasureRoutine.Execute(account, config, out var result, out string error))
                        {
                            switch (error)
                            {
                                case "errServerStatus401":
                                case "errUserNotAuthorized":
                                case "errRequestMaintenance":
                                case "errRequestOutdatedClientVersion":
                                case "errServerStatus400": // bad request
                                case "errServerStatus403": // forbidden - introduced by cloudflare
                                case "errServerStatus429": // too many requests
                                case "errServerStatus504": // service unavailable
                                case "errRequestBlocked":
                                    account.Status = Account.AccountStatus.Unauthorized;
                                    _accounts[account] = UnixTime.Now();
                                    break;

                                default:
                                    account.Logger.Warn("Treasure fail: " + error);
                                    account.InternalState!.IsTreasureFailed = true;
                                    break;
                            }
                        }
                    }

                    if (cancellationToken.IsCancellationRequested) return;

                    //claim hero book before daily reward

                    if (account.Status == Account.AccountStatus.Active && config.ClaimDailyBonusRewards && !account.InternalState!.IsDailyBonusFailed)
                    {
                        if (!DailyRewardRoutine.Execute(account, config, out var result, out string error))
                        {
                            switch (error)
                            {
                                case "errServerStatus401":
                                case "errUserNotAuthorized":
                                case "errRequestMaintenance":
                                case "errRequestOutdatedClientVersion":
                                case "errServerStatus400": // bad request
                                case "errServerStatus403": // forbidden - introduced by cloudflare
                                case "errServerStatus429": // too many requests
                                case "errServerStatus504": // service unavailable
                                case "errRequestBlocked":
                                    account.Status = Account.AccountStatus.Unauthorized;
                                    _accounts[account] = UnixTime.Now();
                                    break;

                                default:
                                    account.Logger.Warn("daily bonus fail: " + error);
                                    account.InternalState!.IsDailyBonusFailed = true;
                                    break;
                            }
                        }
                    }

                    if (account.Status == Account.AccountStatus.Unauthorized) continue;

                    account.Status = Account.AccountStatus.Ready;
                    await Task.Delay(TimeSpan.FromSeconds(0.5), cancellationToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }, cancellationToken);
        }

        public void Abort()
        {
            if (!Running)
            {
                return;
            }

            _cancellationTokenSource?.Cancel();
            Running = false;
            FileLogger.Instance.Info("Aborting execution now, once scheduled routines are finished execution will be stopped.");
        }
    }
}
