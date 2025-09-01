using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Request;
using HeroEngine.Request.Treasure;
using HeroEngine.Util;

namespace HeroEngine.Routine
{
    public class TreasureRoutine : Routine<RoutineResult>
    {

        protected ExecutionConfiguration _config;
        public TreasureRoutine(Account account, ExecutionConfiguration config) : base(account)
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

            if (!_config.Treasure)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            var data = _account.HeroZero!.Data;

            if (data.ActiveTreasureEvent == null)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            if (data.ActiveTreasureEvent.DateExpires <= DateTime.Now)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            if (UnixTime.Since(data.ActiveTreasureEvent.TimeShovelsCollected) < HeroZero.Constants.TreasureEventCollectCooldown)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            //assign treasure event

            if (!data.Character.TutorialFlagsJson.Contains("tutorial_event_treasure"))
            {
                if (!new SetTutorialFlags(_account, "tutorial_event_treasure", "true").Execute(out var flagData, out string flagError))
                {
                    _account.Logger.Warn($"Set treasure tutorial flag error: {flagError}");

                    result = RoutineResult.UnhandledError;
                    error = flagError;
                    return false;
                }

                SetTutorialFlags.Update(_account, flagData);
                _account.Logger.Info($"Set treasure event tutorial flag");

                if (!new SyncGame(_account, false).Execute(out var syncData, out var syncError))
                {
                    _account.Logger.Warn($"Unable to synchronize game after setting treasure tutorial flag: {syncError}");
                }

                SyncGame.Update(_account, syncData);
                _account.Logger.Info($"Synchronized game");
            }


            //errClaimFreeTreasureRevealItemsInvalidEventId
            // maybe we are missing event flag for being viewed?
            if (!new ClaimFreeTreasureRevealItems(_account).Execute(out var claimData, out string claimError))
            {
                _account.Logger.Warn($"Unable to collect shovels: {claimError}");

                result = RoutineResult.UnhandledError;
                error = claimError;
                return false;
            }

            ClaimFreeTreasureRevealItems.Update(_account, claimData);
            _account.Logger.Info($"Collected {data.ActiveTreasureEvent.Shovels} shovels so far");

            result = RoutineResult.Finished;
            error = "";
            return true;
        }

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            return new TreasureRoutine(account, config).Execute(out result, out error);
        }
    }
}
