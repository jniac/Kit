using System;
namespace Kit
{
    public partial class Event
    {
        static object defaultKey;

        public class DisposableKey : IDisposable
        {
            object previousDefaultKey;

            public DisposableKey(object key)
            {
                previousDefaultKey = defaultKey;
                defaultKey = key;
            }

            public void Dispose()
            {
                defaultKey = previousDefaultKey;
				previousDefaultKey = null;
            }
        }

        public static DisposableKey WithKey(object key) =>
            new DisposableKey(key);

        public static void WithKey(object key, Action callback)
        {
            object previousDefaultKey = defaultKey;
            defaultKey = key;

            callback();

            defaultKey = previousDefaultKey;
        }

        public static void WithKey(object key, params Listener[] listeners)
        {
            foreach (var listener in listeners)
                foreach (var child in listener.GetDescendants(true))
                    child.key = key;
        }
    }
}
