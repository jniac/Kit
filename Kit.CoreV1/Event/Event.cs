using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Kit.CoreV1
{
    public enum EventPhase
    {
        NONE,
        ENTER,
        EXIT,
    }

    public partial class Event : IEvent
    {
        public const string global = "global";

        static string ToReadableTypeName(Type type)
        {
            var parents = new List<Type>();
            var parent = type.ReflectedType;

            while (parent != null)
            {
                parents.Insert(0, parent);
                parent = parent.ReflectedType;
            }

            string prefix = string.Join(".", parents.Select(t => t.Name)) + (parents.Count > 0 ? "." : "");

            if (type.IsGenericType)
            {
                string g = string.Join(", ", type.GetGenericArguments().Select(t => t.Name));
                return $"{prefix}{type.Name.Split('`')[0]}<{g}>";
            }

            return $"{prefix}{type.Name}";
        }

        static int eventCount;
        public readonly int id = eventCount++;

        public bool Locked { get; private set; } = false;

        protected object target;
        public object Target
        {
            get => target;
            set { if (!Locked) target = value; }
        }

        protected IList targets;
        public IList Targets
        {
            get => targets;
            set { if (!Locked) { targets = value; target = value; } }
        }

        protected object currentTarget;
        public object CurrentTarget { get => currentTarget; }

        public string Name { get; private set; }

        protected object type;
        public object Type
        {
            get => type;
            set { if (!Locked) type = value; }
        }

        protected bool consumable;
        public bool Comsumable
        {
            get => consumable;
            set { if (!Locked) consumable = value; }
        }
        public bool Consumed { get; private set; } = false;
        public void Consume() => Consumed = Comsumable;

        public EventPhase Phase { get; set; } = EventPhase.NONE;
        public bool Enter
        {
            get => Phase == EventPhase.ENTER;
            set { Phase = value ? EventPhase.ENTER : EventPhase.EXIT; }
        }
        public bool Exit
        {
            get => Phase == EventPhase.EXIT;
            set { Phase = value ? EventPhase.EXIT : EventPhase.ENTER; }
        }

        public bool StartsGlobal { get; set; } = false;
        public bool EndsGlobal { get; set; } = false;

        public Func<object, object> Propagation { get; set; } = null;
        protected virtual object InvokePropagation(object t)
            => Propagation == null ? null : Propagation(t);

        public Event()
        {
            target = global;
            type = Name = ToReadableTypeName(GetType());
        }

        public override string ToString()
        {
            string phaseStr = Enter ? ", Enter" : Exit ? ", Exit" : "";

            return $"{Name}#{id}({target}, {type}{phaseStr})";
        }
    }

    public class Event<T> : Event
        where T : class
    {
        public new T Target
        {
            get => target as T;
            set { if (!Locked) target = value; }
        }

        public new T CurrentTarget { get => currentTarget as T; }

        public new Func<T, object> Propagation { get; set; } = null;
        protected override object InvokePropagation(object t)
            => Propagation == null ? null : Propagation(t as T);
    }
}
