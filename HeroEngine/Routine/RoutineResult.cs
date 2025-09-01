namespace HeroEngine.Routine
{
    public enum RoutineResult
    {
        Uninitialized,
        Unauthorized,

        Maintenance,
        OutdatedVersion,
        Blocked,

        UnhandledError,

        Sleeping,
        Finished
    }
}
