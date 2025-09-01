using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request
{
    // maximum 6 logins per account, on 7th you cannot login, ban expires in 3 hours
    // autologins dont have that limitation
    public class LoginUser : Request
    {
        public string ClientId;
        public LoginUser(Account account) : base(account, "loginUser")
        {
            if (account.Session != null && !string.IsNullOrEmpty(account.Session.ClientId))
            {
                ClientId = account.Session.ClientId;
                return;
            }

            ClientId = $"{account.Server.ToLower()}{UnixTime.Now()}";
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["email"] = _account.Email;
            data["password"] = _account.Password;
            data["platform"] = "";
            data["platform_user_id"] = "";
            data["client_id"] = $"{_account.Session?.ClientId}";
            data["app_version"] = HeroZero.Version;
            data["device_info"] = HeroZero.DeviceInfo;
            data["device_id"] = "web";

            // override those parameters
            data["user_id"] = "0";
            data["user_session_id"] = "0";
            data["auth"] = HeroZero.Hash(Action, 0);

            return data;
        }

        public static void Update(Account account, dynamic data)
        {
            if (data == null) return;

            var hz = account.HeroZero;
            if (hz == null) return;

            var game = hz.Data;
            if (game == null) return;

            JsonPropertyUpdater.UpdateFields(game, data);

            account.Name = game.Character.Name;

            account.Session ??= new Account.ExistingSession();
            account.Session.UserId = game.User.Id;
            account.Session.SessionId = game.User.SessionId;
        }
    }
}
