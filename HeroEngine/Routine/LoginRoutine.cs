using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Request;
using HeroEngine.Util;

namespace HeroEngine.Routine
{
    public class LoginRoutine : Routine<RoutineResult>
    {
        public LoginRoutine(Account account) : base(account)
        {
        }

        public override bool Execute(out RoutineResult result, out string error)
        {
            _account.HeroZero = new HeroZero();

            if (_account.Session != null)
            {
                if (new AutoLoginUser(_account).Execute(out var sessionData, out string sessionError))
                {
                    AutoLoginUser.Update(_account, sessionData);
                    _account.Status = Account.AccountStatus.Ready;
                    _account.InternalState = new InternalStateHolder();
                    _account.HeroZero.TsLastSync = UnixTime.Now();
                    _account.HeroZero.TsLastGuildSync = UnixTime.Now();

                    result = RoutineResult.Finished;
                    error = "";
                    return true;
                }
            }

            string clientId = _account.Session?.ClientId ?? "";
            clientId = !string.IsNullOrEmpty(clientId) ? clientId : $"{_account.Server.ToLower()}{UnixTime.Now()}";
            _account.Session ??= new Account.ExistingSession() { ClientId = clientId };

            if (new LoginUser(_account).Execute(out var loginData, out string loginError))
            {
                LoginUser.Update(_account, loginData);
                _account.Status = Account.AccountStatus.Ready;
                _account.InternalState = new InternalStateHolder();
                _account.HeroZero.TsLastSync = UnixTime.Now();
                _account.HeroZero.TsLastGuildSync = UnixTime.Now();

                result = RoutineResult.Finished;
                error = "";
                return true;
            }
            else
            {
                _account.Status = Account.AccountStatus.Undetermined;
                result = RoutineResult.UnhandledError;
                error = loginError;

                if (loginError.StartsWith("errLoginTempBanStatus_"))
                {
                    _account.Status = Account.AccountStatus.Suspended;
                    result = RoutineResult.Unauthorized;
                    return false;
                }

                switch (loginError)
                {
                    case "errLoginInvalidEmail":
                    case "errLoginNoSuchUser":
                        _account.Status = Account.AccountStatus.DoesNotExist;
                        break;

                    case "errLoginInvalidStatus": // konto nie jest juz aktywne
                        _account.Status = Account.AccountStatus.Deleted;
                        break;

                    case "errLoginInvalid": // konto jest na tempbanie za duzo logowan
                        _account.Status = Account.AccountStatus.Unauthorized;
                        result = RoutineResult.Unauthorized;
                        break;

                    case "errRequestOutdatedClientVersion":
                        result = RoutineResult.OutdatedVersion;
                        break;

                    case "errRequestMaintenance":
                        result = RoutineResult.Maintenance;
                        break;

                    case "errServerStatus400": // bad request
                    case "errServerStatus403": // forbidden - introduced by cloudflare
                    case "errServerStatus429": // too many requests
                    case "errServerStatus504": // service unavailable
                    case "errRequestBlocked":
                        result = RoutineResult.Blocked;
                        break;
                }

                return false;
            }
        }

        public static bool Execute(Account account, out RoutineResult result, out string error)
        {
            return new LoginRoutine(account).Execute(out result, out error);
        }
    }
}
