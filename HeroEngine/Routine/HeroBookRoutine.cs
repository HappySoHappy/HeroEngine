using HeroEngine.Persistance;

namespace HeroEngine.Routine
{
    public class HeroBookRoutine
    {

        public static bool Execute(Account account, ExecutionConfiguration config, out RoutineResult result, out string error)
        {
            result = RoutineResult.Uninitialized;
            error = string.Empty;
            return false;
        }
    }
}
