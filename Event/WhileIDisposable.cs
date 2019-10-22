using System;

namespace Kit
{
    public partial class Event
    {
        public class DisposableWhile<T> : IDisposable
            where T : class, IEvent
        {
            Listener master;

            public DisposableWhile(object target, object type, bool childrenDisabled)
            {
                master = On<T>(target, type,
                    enter: e => master.ChildrenDisabled = false,
                    exit: e => master.ChildrenDisabled = true);

                master.IsWhileContainer = true;
                master.ChildrenDisabled = childrenDisabled;

                Listener.OnNewListener.Add(OnNewListener);
            }

            void OnNewListener(Listener listener)
            {
                master.AddChild(listener);
            }

            public void Dispose()
            {
                Listener.OnNewListener.Remove(OnNewListener);
            }
        }

        public static DisposableWhile<T> WhileIDisposable<T>(object target, object type, bool childrenDisabled = true)
            where T : class, IEvent =>
            new DisposableWhile<T>(target, type, childrenDisabled);

        public static DisposableWhile<T> WhileIDisposable<T>(object target, bool childrenDisabled = true)
            where T : class, IEvent =>
            new DisposableWhile<T>(target, "*", childrenDisabled);

        public static DisposableWhile<T> WhileIDisposable<T>(bool childrenDisabled = true)
            where T : class, IEvent =>
            new DisposableWhile<T>(global, "*", childrenDisabled);
    }
}
