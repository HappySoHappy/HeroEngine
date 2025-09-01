using HeroEngine.Framework;
using HeroEngine.Persistance;

namespace HeroEngine.Request
{
    public abstract class Request
    {
        protected Account _account;
        public string Action { get; private set; }
        public Request(Account account, string action)
        {
            _account = account;
            Action = action;
        }

        public abstract RequestData Create();

        public bool Execute(out dynamic data, out string error)
        {
            data = _account.HeroZero!.PostActionRequest(_account, this, out error);
            return string.IsNullOrEmpty(error) && data != null;
        }
    }
}
