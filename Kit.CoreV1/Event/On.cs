using System;
namespace Kit.CoreV1
{
    public partial class Event
    {
		public static Listener On(object target, object type,
            Action<Event> callback = null,
            Action<Event> enter = null,
            Action<Event> exit = null,
            object key = null,
            int priority = PRIORITY_NORMAL)
            => new Listener(target, type, key, callback, enter, exit, typeof(Event), priority);

        public static Listener On(object target,
            Action<Event> callback = null,
            Action<Event> enter = null,
            Action<Event> exit = null,
            object key = null,
            int priority = PRIORITY_NORMAL)
            => new Listener(target, "*", key, callback, enter, exit, typeof(Event), priority);

        public static Listener<T> On<T>(object target, object type,
            Action<T> callback = null,
            Action<T> enter = null,
            Action<T> exit = null,
            object key = null,
            int priority = PRIORITY_NORMAL)
            where T : class, IEvent
        {
            Action<Event> callbackT = null, enterT = null, exitT = null;

            if (callback != null)
                callbackT = e => callback(e as T);

            if (enter != null)
                enterT = e => enter(e as T);

            if (exit != null)
                exitT = e => exit(e as T);

            return new Listener<T>(target, type, key, callbackT, enterT, exitT, typeof(T), priority);
        }

        public static Listener<T> On<T>(object target,
            Action<T> callback = null,
            Action<T> enter = null,
            Action<T> exit = null,
            object key = null,
            int priority = PRIORITY_NORMAL)
            where T : class, IEvent
            => On<T>(target, "*", callback, enter, exit, key, priority);

        public static Listener<T> On<T>(
            Action<T> callback = null,
            Action<T> enter = null,
            Action<T> exit = null,
            object key = null,
            int priority = PRIORITY_NORMAL)
            where T : class, IEvent
            => On<T>(global, "*", callback, enter, exit, key, priority);
    }
}
