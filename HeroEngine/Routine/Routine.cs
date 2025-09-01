using HeroEngine.Persistance;

namespace HeroEngine.Routine
{
    public abstract class Routine<T> where T : Enum, new()
    {
        protected Account _account;
        public Routine(Account account)
        {
            _account = account;
        }

        public abstract bool Execute(out T result, out string error);
    }
}
