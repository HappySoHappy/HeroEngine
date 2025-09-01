using HeroEngine.Persistance;
using HeroEngine.Request;
using HeroEngine.Util;

namespace HeroEngine.Routine
{
    public class SynchronizeRoutine : Routine<RoutineResult>
    {
        public SynchronizeRoutine(Account account) : base(account)
        {
        }

        public override bool Execute(out RoutineResult result, out string error)
        {
            if (new SyncGame(_account).Execute(out var syncData, out string syncError))
            {
                SyncGame.Update(_account, syncData);
                _account.HeroZero!.TsLastSync = UnixTime.Now();

                _account.Logger.Info("Synchronized game");

                result = RoutineResult.Finished;
                error = "";
                return true;
            }
            else
            {
                _account.Status = Account.AccountStatus.Unauthorized;
                result = RoutineResult.UnhandledError;
                error = syncError;

                switch (syncError)
                {
                    case "errServerStatus401":
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
                    case "errServerStatus504": // service unavailable
                    case "errRequestBlocked":
                        result = RoutineResult.Blocked;
                        break;
                    default:
                        _account.Logger.Warn($"Unable to synchronize game: {syncError}");
                        break;
                }
            }

            return false;
        }

        public static bool Execute(Account account, out RoutineResult result, out string error)
        {
            return new SynchronizeRoutine(account).Execute(out result, out error);
        }
    }
}
