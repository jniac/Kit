using System;
namespace Kit.CoreV1
{
    public partial class Event
    {
        public static Listener Once(object target, object type,
            Action<Event> callback = null,
            Action<Event> enter = null,
            Action<Event> exit = null,
            object key = null,
            int priority = PRIORITY_NORMAL)
        {
            var listener = On(target, type, callback, enter, exit, key, priority);
            listener.maxInvokeCount = 1;
            return listener;
        }

        public static Listener Once<T>(object target, object type,
            Action<T> callback = null,
            Action<T> enter = null,
            Action<T> exit = null,
            object key = null,
            int priority = PRIORITY_NORMAL)
            where T : class, IEvent
        {
            var listener = On<T>(target, type, callback, enter, exit, key, priority);
            listener.maxInvokeCount = 1;
            return listener;
        }
    }
}
