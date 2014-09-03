namespace WindViewer.Editor
{
    public class Singleton<T> where T : Singleton<T>
    {
        private static T _cachedCopy;
        public Singleton()
        {
            _cachedCopy = (T)this;
        }

        public static T Instance
        {
            get { return _cachedCopy; }
        }
    }
}