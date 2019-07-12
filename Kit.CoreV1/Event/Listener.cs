using System;
using System.Linq;
using System.Collections.Generic;
namespace Kit.CoreV1
{
    public partial class Event
    {
        public class Listener
        {
            static RegisterWithOptionalKey<Listener, object, object> register =
                new RegisterWithOptionalKey<Listener, object, object>();

            public static int Count { get => register.Count; }
            public static string Info { get => register.Info; }

            public static IEnumerable<Listener> Get(object target, Event e) =>
                register.Get(target)
                .Where(lsn => lsn.eventType.IsInstanceOfType(e) && lsn.MatchType(e.type));

            public static Listener[] ByTarget(object target) =>
                register.Get(target).ToArray();

            public static Listener[] ByKey(object key) =>
                register.GetWithOptionalKey(key).ToArray();

            static int listenerCount;
            public readonly int id = listenerCount++;

            public readonly object target, type, key;
            public readonly Type eventType;

            public bool Destroyed { get; private set; } = false;

            public int InvokeCount { get; private set; } = 0;
            public int maxInvokeCount = 0;

            List<Listener> nested = new List<Listener>();

            public readonly Action<Event> callback, enter, exit;

            public Listener(object target, object type, object key, 
                Action<Event> callback, 
                Action<Event> enter, 
                Action<Event> exit,
                Type eventType)
            {
                this.target = target ?? global;
                this.type = type ?? "*";
                this.key = key;

                this.callback = callback;
                this.enter = enter;
                this.exit = exit;

                this.eventType = eventType;

                register.Add(this, target, key);
            }

            public bool MatchType(object otherType)
            {
                if (type.Equals("*") || otherType.Equals("*"))
                    return true;

                return type.Equals(otherType);
            }

            public void Invoke(Event e)
            {
                callback?.Invoke(e);

                if (e.Enter)
                    enter?.Invoke(e);

                if (e.Exit)
                    exit?.Invoke(e);

                InvokeCount++;

                if (maxInvokeCount == InvokeCount)
                    Destroy();
            }

            public void Destroy()
            {
                register.Remove(this, target, key);

                Destroyed = true;
            }

            public override string ToString()
            {
                string f(object o) => o.Equals(null) ? "no" : "yes";

                return $"Listener#{id}({target}, {type}, {eventType}, " +
                	$"callback:{f(callback)}, enter:{f(enter)}, exit:{f(exit)})";
            }
        }
    }
}
