using HeroEngine.Framework;
using HeroEngine.Persistance;

namespace HeroEngine.Request
{
    public class AutoLoginUser : Request
    {
        public AutoLoginUser(Account account) : base(account, "autoLoginUser")
        {
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            if (_account.Session == null) throw new Exception("Account session is null, unable to create valid RequestData!");

            data["existing_session_id"] = _account.Session.SessionId;
            data["existing_user_id"] = _account.Session.UserId;
            data["client_id"] = _account.Session.ClientId;
            data["app_version"] = HeroZero.Version;

            // override those parameters
            data["user_id"] = "0";
            data["user_session_id"] = "0";
            data["auth"] = HeroZero.Hash(Action, 0);

            return data;
        }

        public static void Update(Account account, dynamic data)
        {
            LoginUser.Update(account, data);
        }
    }
}
