using System;
using System.Linq;
using System.Collections.Generic;
namespace Kit.CoreV1
{
    public partial class Event
    {
        public const int PRIORITY_HIGHER =  2000;
        public const int PRIORITY_HIGH =    1000;
        public const int PRIORITY_NORMAL =  0;
        public const int PRIORITY_LOW =     -1000;
        public const int PRIORITY_LOWER =   -2000;

        public partial class Listener
        {
            static RegisterWithOptionalKey<Listener, object, object> register =
                new RegisterWithOptionalKey<Listener, object, object>();

            public static int TotalCount { get => register.TotalCount; }
            public static string Info { get => $"Listeners: {register.TotalCount}, keys: {register.Keys.Count()}"; }
            public static string InfoAllListener { get => register.InfoAllValues; }
            public static string InfoAllKeys { get => string.Join("\n", register.Keys.Select((v, i) => $"{i}: {v}")); }

            public static Listener[] Get(object target, Event e)
            {
                Listener[] listeners = register.Get(target)
                    // NOTE: order is important for optimization
                    .Where(lsn => lsn.eventType.IsInstanceOfType(e) && lsn.MatchType(e.type) && lsn.IsEnabled)
                    .ToArray();

                Array.Sort(listeners, (a, b) => b.priority - a.priority);

                return listeners;
            }

            public static Listener[] ByTarget(object target) =>
                register.Get(target).ToArray();

            public static Listener[] ByKey(object key) =>
                register.GetWithOptionalKey(key).ToArray();

            public static HashSet<Action<Listener>> OnNewListener = new HashSet<Action<Listener>>();



            static int listenerCount;
            public readonly int id = listenerCount++;

            public object key;
            public readonly object target, type;
            public readonly Type eventType;

            public bool Destroyed { get; private set; } = false;

            public int InvokeCount { get; private set; } = 0;
            public int maxInvokeCount = 0;

            public readonly Action<Event> callback, enter, exit;
            public int priority;
            public bool consume;

            public Listener(object target, object type, object key, 
                Action<Event> callback, 
                Action<Event> enter, 
                Action<Event> exit,
                Type eventType,
                int priority, 
                bool consume)
            {
                this.target = target ?? global;
                this.type = type ?? "*";
                this.key = key ?? defaultKey;

                this.callback = callback;
                this.enter = enter;
                this.exit = exit;

                this.eventType = eventType;

                this.priority = priority;
                this.consume = consume;

                register.Add(this, this.target, this.key);

                foreach (var action in OnNewListener)
                    action(this);
            }

            public bool MatchType(object otherType)
            {
                if (type.Equals("*") || otherType.Equals("*"))
                    return true;

                return type.Equals(otherType);
            }

            public Event Invoke(Event e)
            {
                callback?.Invoke(e);

                if (e.Enter)
                    enter?.Invoke(e);

                if (e.Exit)
                    exit?.Invoke(e);

                if (consume)
                    e.Consume();

                DisposeAwaiters(e);

                InvokeCount++;

                if (maxInvokeCount == InvokeCount)
                    Destroy();

                return e;
            }

            public void Destroy(bool throwIfAlreadyDestroyed = true)
            {
                if (Destroyed)
                {
                    if (throwIfAlreadyDestroyed)
                    {
                        string info(object obj) => $"{(obj != null ? obj.GetType().Name : "null")}|{obj}";
                        throw new Exception($"Listener has already been destroyed ({info(target)}, {info(key)})");
                    }

                    return;
                }

                register.Remove(this, target, key);

                foreach (var child in children)
                    child.Destroy(throwIfAlreadyDestroyed);

                children.Clear();

                Destroyed = true;
            }

            public override string ToString()
            {
                string f(object o) => o == null ? "no" : "yes";

                return $"Listener#{id}({target}, {type}, {eventType}, key:{key}, " +
                	$"callback:{f(callback)}, enter:{f(enter)}, exit:{f(exit)})";
            }
        }
    }
}
