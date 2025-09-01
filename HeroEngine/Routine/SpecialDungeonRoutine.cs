using HeroEngine.Framework;
using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Request;
using HeroEngine.Request.Dungeon;
using HeroEngine.Util;
using Newtonsoft.Json;
namespace HeroEngine.Routine
{
    public class SpecialDungeonRoutine : Routine<RoutineResult>
    {
        protected ExecutionConfiguration _config;
        public SpecialDungeonRoutine(Account account, ExecutionConfiguration config) : base(account)
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


            if (UnixTime.Since(data.Character.TimeDungeonFailed) < 3600)
            {
                result = RoutineResult.Sleeping;
                error = "";
                return true;
            }

            //ActiveDungeonQuestId
            //ActiveDungeonAttackId


            if (data.Character.ActiveMissionId != 0)
            {
                result = RoutineResult.Sleeping;
                error = "";
                return true;
            }

            if (data.Inventory.Throwable == 0)
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
                        _account.Logger.Info("Equipped throwables for dungeons run");
                    }
                    else
                    {
                        _account.Logger.Warn($"Unable to equip throwables for dungeons run, {moveItemError}");
                    }
                }
            }

            List<Dungeon> dungeons = new List<Dungeon>();
            using (List<Dungeon>.Enumerator enumerator = data.Dungeons.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Dungeon dungeon = enumerator.Current;
                    bool flag = dungeon.Mode == 2 && dungeon.Status == 3;
                    if (flag)
                    {
                        int num = dungeons.FindIndex((Dungeon item) => string.Equals(item.Identifier, dungeon.Identifier));
                        bool flag2 = num >= 0;
                        if (flag2)
                        {
                            dungeons.RemoveAt(num);
                            dungeons.Add(dungeon);
                        }
                    }
                    bool flag3 = dungeon.Mode == 2 && dungeon.Status == 4;
                    if (flag3)
                    {
                        int num2 = dungeons.FindIndex((Dungeon item) => string.Equals(item.Identifier, dungeon.Identifier));
                        bool flag4 = num2 >= 0;
                        if (flag4)
                        {
                            bool flag5 = dungeon.TimeCompleted > dungeons[num2].TimeCompleted && dungeon.TimeCompleted != 0;
                            if (flag5)
                            {
                                dungeons.RemoveAt(num2);
                                dungeons.Add(dungeon);
                            }
                        }
                        else
                        {
                            dungeons.Add(dungeon);
                        }
                    }
                }
            }

            //dungeon can be restarted once every 432000s
            //cooldown when there is herocon for dungeons: 86400
            long cooldown = data.GuildCompetition?.Identifier == "default_guild_competition_complete_dungeon_quest" ? HeroZero.Constants.DungeonRepeatShortCooldown : HeroZero.Constants.DungeonRepeatCooldown;
            foreach (var dungeon in dungeons)
            {
                //dungeon.Status != 3 && dungeon.TimeCompleted != 0 && dungeon.CurrentQuestId != 0
                if (dungeon.ProgressIndex == 10 && UnixTime.Since(dungeon.TimeCompleted) > cooldown)
                {
                    //restart dungeon
                    if (new RestartDungeon(_account, dungeon.Id).Execute(out var restartData, out string restartError))
                    {
                        RestartDungeon.Update(_account, restartData);
                        _account.Logger.Info($"Started dungeon {dungeon.GetDungeonName()}");
                    } else
                    {
                        //errRestartDungeonActiveCooldown
                        //ClaimDungeonQuestRewards check what data this has, if current quest id is 0 or something on last quest, dont do anything
                        _account.Logger.Warn($"Unable to start dungeon {dungeon.GetDungeonName()}, {restartError}");
                        continue;
                    }
                }

                for (int i = dungeon.ProgressIndex + 1; i <= 10; i++)
                {
                    if (dungeon.CurrentQuestId == 0) continue;
                    /*if (new StartDungeonQuest(_account, dungeon.CurrentQuestId).Execute(out var startQuestData, out string startQuestError))
                    {
                        StartDungeonQuest.Update(_account, startQuestData);

                        Battle battle = JsonConvert.DeserializeObject<Battle>(JsonConvert.SerializeObject(startQuestData.battle));
                        if (battle.Winner != "a")
                        {
                            _account.Logger.Info($"Lost dungeon fight #{i}/10 on {dungeon.GetDungeonName()}");

                            result = RoutineResult.Finished;
                            error = "";
                            return true;
                        }

                        _account.Logger.Info($"Won dungeon fight #{i}/10 on {dungeon.GetDungeonName()}");

                        if (new FinishDungeonQuest(_account).Execute(out var finishQuestData, out string finishQuestError))
                        {
                            FinishDungeonQuest.Update(_account, finishQuestData);

                            //get item, and if its throwables, save them - only sell if we dont have space in inventory
                            if (new ClaimDungeonQuestRewards(_account, true).Execute(out var claimQuestData, out string claimQuestError))
                            {
                                ClaimDungeonQuestRewards.Update(_account, claimQuestData);
                            }
                        } else
                        {
                            _account.Logger.Warn($"Unable to finish dungeon fight #{i}/10 on {dungeon.GetDungeonName()}, {finishQuestError}");
                            goto NextDungeon;
                        }
                    } else
                    {
                        //errStartDungeonQuestActiveDungeonQuestFound
                        _account.Logger.Warn($"Unable to start dungeon fight #{i}/10 on {dungeon.GetDungeonName()}, {startQuestError}");
                        goto NextDungeon;
                    }*/

                    if (data.Character.ActiveDungeonQuestId == 0)
                    {
                        if (new StartDungeonQuest(_account, dungeon.CurrentQuestId).Execute(out var startQuestData, out string startQuestError))
                        {
                            StartDungeonQuest.Update(_account, startQuestData);

                            Battle battle = JsonConvert.DeserializeObject<Battle>(JsonConvert.SerializeObject(startQuestData.battle));
                            if (battle.Winner != "a")
                            {
                                _account.Logger.Info($"Lost dungeon fight #{i}/10 on {dungeon.GetDungeonName()}");

                                result = RoutineResult.Finished;
                                error = "";
                                return true;
                            }

                            _account.Logger.Info($"Won dungeon fight #{i}/10 on {dungeon.GetDungeonName()}");
                        }
                        else
                        {
                            //errStartDungeonQuestActiveDungeonQuestFound
                            _account.Logger.Warn($"Unable to start dungeon fight #{i}/10 on {dungeon.GetDungeonName()}, {startQuestError}");
                            goto NextDungeon;
                        }

                        if (new FinishDungeonQuest(_account).Execute(out var finishQuestData, out string finishQuestError))
                        {
                            FinishDungeonQuest.Update(_account, finishQuestData);
                            //Console.WriteLine($"=== FinishDungeonQuest #{i} - {_account.Name} ===");
                            //Console.WriteLine(finishQuestData);
                            //Console.WriteLine($"===       END Request      ===");


                            //get item, and if its throwables, save them - only sell if we dont have space in inventory
                            if (new ClaimDungeonQuestRewards(_account, true).Execute(out var claimQuestData, out string claimQuestError))
                            {
                                ClaimDungeonQuestRewards.Update(_account, claimQuestData);
                                //Console.WriteLine($"=== ClaimDungeonQuestRewards #{i} - {_account.Name} ===");
                                //Console.WriteLine(claimQuestData);
                                //Console.WriteLine($"===       END Request      ===");
                                //check what data this has, if current quest id is 0 or something, dont do anything!
                                if (i == 10)
                                {
                                    //WHAT IF WE FAILED THE FIGHT?????
                                    dungeon.TimeCompleted = UnixTime.Now(); // hack to fix it
                                    dungeon.CurrentQuestId = 0;
                                    dungeon.ProgressIndex = 10;
                                }
                            }
                        }
                        else
                        {
                            _account.Logger.Warn($"Unable to finish dungeon fight #{i}/10 on {dungeon.GetDungeonName()}, {finishQuestError}");
                            goto NextDungeon;
                        }
                    }

                NextDungeon:
                    continue;
                }
            }

            result = RoutineResult.Finished;
            error = "";
            return true;
        }

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            return new SpecialDungeonRoutine(account, config).Execute(out result, out error);
        }
    }
}
