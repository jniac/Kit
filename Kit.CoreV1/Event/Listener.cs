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

            public static int TotalCount { get => register.TotalCount; }
            public static string Info { get => register.Info; }

            public static IEnumerable<Listener> Get(object target, Event e) =>
                register.Get(target)
                .Where(lsn => lsn.eventType.IsInstanceOfType(e) && lsn.MatchType(e.type) && lsn.IsEnabled);

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

            public readonly Action<Event> callback, enter, exit;

            public Listener(object target, object type, object key, 
                Action<Event> callback, 
                Action<Event> enter, 
                Action<Event> exit,
                Type eventType)
            {
                this.target = target ?? global;
                this.type = type ?? "*";
                this.key = key ?? defaultKey;

                this.callback = callback;
                this.enter = enter;
                this.exit = exit;

                this.eventType = eventType;

                register.Add(this, this.target, this.key);
            }


            // While / Phase implementation (tree) >
            public bool ChildrenDisabled { get; set; } = false;
            List<Listener> children = new List<Listener>();
            Listener parent, root;
            public void AddChild(Listener child)
            {
                child.root = root ?? this;
                child.parent = this;
            }
            public bool GetIsEnabled()
            {
                Listener current = parent;

                while (current != null)
                {
                    if (current.ChildrenDisabled)
                        return false;

                    current = current.parent;
                }

                return true;
            }
            public bool IsEnabled { get => GetIsEnabled(); }

            public static Listener operator +(Listener lhs, Listener rhs)
            {
                lhs.AddChild(rhs);

                return lhs;
            }
            // While / Phase implementation (tree) <



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
                string f(object o) => o == null ? "no" : "yes";

                return $"Listener#{id}({target}, {type}, {eventType}, key:{key}, " +
                	$"callback:{f(callback)}, enter:{f(enter)}, exit:{f(exit)})";
            }
        }
    }
}
