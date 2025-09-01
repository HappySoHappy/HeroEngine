namespace HeroEngine.Desktop.Interface
{
    public interface ITranslatable
    {

        private static HashSet<ITranslatable> _subscribers = new HashSet<ITranslatable>();

        public static void Subscribe(ITranslatable subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public static void Unsubscribe(ITranslatable subscriber)
        {
            _subscribers.Remove(subscriber);
        }

        public static void NotifyChange(Language newTable)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.OnLanguageChanged(newTable);
            }
        }

        public abstract void OnLanguageChanged(Language newTable);
    }
}
