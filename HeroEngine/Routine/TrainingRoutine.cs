using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Request;
using HeroEngine.Request.Training;
using HeroEngine.Util;

namespace HeroEngine.Routine
{
    public class TrainingRoutine : Routine<RoutineResult>
    {
        protected ExecutionConfiguration _config;
        public TrainingRoutine(Account account, ExecutionConfiguration config) : base(account)
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

            if (!_config.Training)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            var data = _account.HeroZero!.Data;

            if (data.Character.TrainingMotivationEnergy <= 0 && _config.TrainingCouponPreferredSelect != ExecutionConfiguration.TrainingCouponSelect.None)
            {
                var trainingVouchers = data.Vouchers
                    .Where(v => v.IsTrainingVoucher() && !v.IsExpired())
                    .OrderBy(v => v.TimeExpires)
                    .ThenByDescending(v => v.Rewards.TrainingProgress);

                if (trainingVouchers.Any())
                {
                    var voucher = trainingVouchers.First();

                    if (UnixTime.Since(data.Character.TimeTrainingFinished) > HeroZero.Constants.TrainingCooldown)
                    {
                        int trainingTime = HeroZero.Constants.TrainingCooldown + voucher.Rewards.TrainingMotivation * 300;
                        if (trainingTime <= UnixTime.UntilMidnight() && !_account.InternalState!.IsTrainingVoucherFailed)
                        {
                            if (new RedeemVoucher(_account, voucher.Code).Execute(out var voucherData, out string voucherError))
                            {
                                RedeemVoucher.Update(_account, voucherData);
                                _account.Logger.Info($"Claimed training voucher with {voucher.Rewards.TrainingMotivation} motivation points");
                            } else
                            {
                                _account.InternalState!.IsTrainingVoucherFailed = true;
                                _account.Logger.Warn("claim training voucher fail: " + voucherError);
                            }
                        }
                    }
                }
            }

            if (data.ActiveTraining != null || (UnixTime.Since(data.Character.TimeTrainingFinished) > HeroZero.Constants.TrainingCooldown && data.Character.TrainingMotivationEnergy > 0))
            {
                RefreshTraining();
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            result = RoutineResult.Sleeping;
            error = "";
            return true;
        }

        public void CompareTrainings()
        {
            if (_account.HeroZero?.Data.Trainings != null && _account.HeroZero?.Data.Trainings.Count > 0)
            {
                var select = _config.TrainingPreferredSelect;
                var trainings = _account.HeroZero?.Data.Trainings.Where(training =>
                    (select.HasFlag(ExecutionConfiguration.TrainingSelect.Strength) && training.IsStrengthTraining()) ||
                    (select.HasFlag(ExecutionConfiguration.TrainingSelect.Stamina) && training.IsStaminaTraining()) ||
                    (select.HasFlag(ExecutionConfiguration.TrainingSelect.DodgeRating) && training.IsDodgeTraining()) ||
                    (select.HasFlag(ExecutionConfiguration.TrainingSelect.CriticalRating) && training.IsCriticalTraining())
                );

                if (trainings == null || !trainings.Any())
                {
                    return;
                }

                if (_config.TrainingShortest)
                {
                    trainings = trainings.OrderBy(training => training.MotivationCost);
                } else
                {
                    trainings = trainings.Where(training => _account.HeroZero?.Data.Character.TrainingMotivationEnergy >= training.MotivationCost);
                    trainings = trainings.OrderByDescending(training =>
                        training.FirstMilestoneRewards.StatPoints + training.FirstMilestoneStats +
                        training.SecondMilestoneRewards.StatPoints + training.SecondMilestoneStats +
                        training.ThirdMilestoneRewards.StatPoints + training.ThirdMilestoneStats
                    );
                }

                if (trainings.Any())
                {
                    TRStartTraining(trainings.First().Id);
                }
            }
        }

        public void RefreshTraining()
        {
            if (new RefreshTrainingPool(_account).Execute(out var refresh, out var error))
            {
                RefreshTrainingPool.Update(_account, refresh);
                _account.Logger.Info("Refreshed training pool");

                CompareTrainings();
            }
            else
            {
                switch (error)
                {
                    case "errGenerateNewTrainingsActiveTrainingFound":
                        if (_account.HeroZero!.Data.ActiveTrainingQuests == null)
                        {
                            TRFinishTraining();
                            return;
                        }

                        var viableQuests = _account.HeroZero.Data.ActiveTrainingQuests // training quests can be null, then we gotta use the active quest
                            .Where(q => q.EnergyCost <= _account.HeroZero.Data.Character.TrainingQuestEnergy)
                            .OrderByDescending(q => q.Rewards.TrainingProgress / q.EnergyCost)
                            .ToList();

                        if (viableQuests == null || viableQuests.Count <= 0) return;

                        TRStartTrainingQuest(viableQuests.First().Id);
                        break;
                }
            }
        }

        public void TRStartTraining(int id)
        {
            var training = _account.HeroZero?.Data.Trainings.Where(t => t.Id == id).First();

            if (new StartTraining(_account, training.Id).Execute(out var start, out var error))
            {
                StartTraining.Update(_account, start);
                _account.Logger.Info($"Started {training.GetTrainingType()} training for {training.MotivationCost} motivation points");

                //todo: use constant: "training_energy_refresh_amount_per_minute" = 1
                int secondsUntilExpired = (int)UnixTime.Until(_account.HeroZero.Data.ActiveTraining.TimeExpires);
                int secondsUntilNextPoint = 60 - (int)((UnixTime.Now() - _account.HeroZero.Data.Character.TimeTrainingEnergyChange) % 60);

                var viableQuests = _account.HeroZero.Data.ActiveTrainingQuests // training quests can be null, then we gotta use the active quest
                            .Where(q => q.EnergyCost <= _account.HeroZero.Data.Character.TrainingQuestEnergy)
                            .OrderByDescending(q => q.Rewards.TrainingProgress / q.EnergyCost)
                            .ToList();

                TRStartTrainingQuest(viableQuests.First().Id); // no elements
            } else
            {
                _account.Logger.Warn($"Unable to start training: {error}");
            }
        }

        public void TRStartTrainingQuest(int id)
        {
            if (new StartTrainingQuest(_account, id).Execute(out var quest, out var error))
            {
                StartTrainingQuest.Update(_account, quest);
                _account.Logger.Info($"Started training quest");

                TRClaimTrainingQuest();
            }
            else
            {
                switch (error)
                {
                    case "errStartTrainingQuestInvalidQuest":
                        TRClaimStar();
                        break;
                    case "errStartTrainingQuestNoActiveTraining": // ignore, all training finished
                        break;
                    default:
                        _account.Logger.Warn($"Unable to start training quest: {error}");
                        break;
                }
            }
        }

        public async void TRClaimTrainingQuest()
        {
            if (new ClaimTrainingQuestRewards(_account).Execute(out var rewards, out var error))
            {
                ClaimTrainingQuestRewards.Update(_account, rewards);
                _account.Logger.Info($"Claimed rewards for training quest, current progress is {_account.HeroZero!.Data.ActiveTraining?.Progress} / {_account.HeroZero!.Data.ActiveTraining?.RequiredProgress}");

                TRClaimStar();

                if (_account.HeroZero!.Data.ActiveTrainingQuests == null)
                {
                    TRFinishTraining();
                    return;
                }

                var viableQuests = _account.HeroZero.Data.ActiveTrainingQuests // training quests can be null, then we gotta use the active quest
                            .Where(q => q.EnergyCost <= _account.HeroZero.Data.Character.TrainingQuestEnergy)
                            .OrderByDescending(q => q.Rewards.TrainingProgress / q.EnergyCost)
                            .ToList();

                if (viableQuests == null || viableQuests.Count <= 0) return;

                TRStartTrainingQuest(viableQuests.First().Id);
            }
            else
            {
                switch (error)
                {
                    case "errClaimTrainingQuestRewardsInvalidQuest":
                        TRFinishTraining();
                        break;
                    default:
                        _account.Logger.Warn($"Unable to claim rewards for training quest: {error}");
                        break;
                }
            }
        }

        public void TRClaimStar()
        {
            if (new ClaimTrainingStar(_account, !_account.HeroZero.Data.Inventory.HasEmptyInventorySlot(_account.HeroZero.Data.Character.Level)).Execute(out var star, out var error))
            {
                ClaimTrainingStar.Update(_account, star);
                _account.Logger.Info("Claimed training milestone");

                if (_account.HeroZero.Data.ActiveTraining.ClaimedStars == 3)
                {
                    TRFinishTraining();
                }
            } else
            {
                if (error == "errClaimTrainingStarAlreadyClaimed") return;

                _account.Logger.Warn($"Unable to claim training milestone: {error}");
            }
        }

        public void TRFinishTraining()
        {
            if (new FinishTraining(_account).Execute(out var finish, out var error))
            {
                FinishTraining.Update(_account, finish);
                _account.Logger.Info("Finished training");
            } else
            {
                _account.Logger.Warn($"Unable to finish training: {error}");
            }
        }

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            return new TrainingRoutine(account, config).Execute(out result, out error);
        }
    }
}
