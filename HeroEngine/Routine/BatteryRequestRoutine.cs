using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Request;

namespace HeroEngine.Routine
{
    public class BatteryRequestRoutine : Routine<RoutineResult>
    {
        protected ExecutionConfiguration _config;
        public BatteryRequestRoutine(Account account, ExecutionConfiguration config) : base(account)
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

            if (!_config.AcceptBatteryRequests)
            {
                result = RoutineResult.Finished;
                error = "";
                return true;
            }

            var data = _account.HeroZero!.Data;
            if (data.StreamInfo.TryGetValue("r", out var rDict) &&
                rDict is Dictionary<string, StreamInfo> rSubDict &&
                rSubDict.TryGetValue(data.Character.Id.ToString(), out var request))
            {
                if (request.Unread == 0)
                {
                    result = RoutineResult.Finished;
                    error = "";
                    return true;
                }

                if (!new AcceptAllResourceRequests(_account).Execute(out var acceptData, out var acceptError))
                {
                    _account.Logger.Warn($"Unable to accept battery requests: {acceptError}");

                    result = RoutineResult.UnhandledError;
                    error = acceptError;
                    return false;
                }

                AcceptAllResourceRequests.Update(_account, acceptError);
                _account.Logger.Info($"Accepted {request.Unread} battery requests");
            }

            //send request

            result = RoutineResult.Finished;
            error = "";
            return true;
        }

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            return new BatteryRequestRoutine(account, config).Execute(out result, out error);
        }
    }
}
