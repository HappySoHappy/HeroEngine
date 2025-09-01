namespace HeroEngine.Desktop.Interface
{
    public interface IThemeable
    {
        private static HashSet<IThemeable> _subscribers = new HashSet<IThemeable>();

        public static void Subscribe(IThemeable subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public static void Unsubscribe(IThemeable subscriber)
        {
            _subscribers.Remove(subscriber);
        }

        public static void NotifyChange(Theme newTheme)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.OnThemeChanged(newTheme);
            }
        }

        public abstract void OnThemeChanged(Theme newTheme);
    }
}
