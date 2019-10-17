using System;
using System.Collections.Generic;


namespace Kit.Utils
{
    public class Toggle<T>
    {
        public class Stack
        {
            List<Action> actions;
            List<Action<T>> actionsT;
            List<Action<T, T>> actionsTT;

            public void Add(Action action) =>
                (actions ?? (actions = new List<Action>())).Add(action);

            public void Add(Action<T> action) =>
                (actionsT ?? (actionsT = new List<Action<T>>())).Add(action);

            public void Add(Action<T,T> action) =>
                (actionsTT ?? (actionsTT = new List<Action<T, T>>())).Add(action);

            public void Call(Toggle<T> toggle)
            {
                if (actions != null)
                    foreach (var action in actions)
                        action();

                if (actionsT != null)
                    foreach (var action in actionsT)
                        action(toggle.value);

                if (actionsTT != null)
                    foreach (var action in actionsTT)
                        action(toggle.value, toggle.OldValue);
            }

            public void StackClear()
            {
                actions?.Clear();
                actionsT?.Clear();
                actionsTT?.Clear();

                actions = null;
                actionsT = null;
                actionsTT = null;
            }

            public static Stack operator +(Stack lhs, Action rhs)
            {
                lhs.Add(rhs);

                return lhs;
            }

            public static Stack operator +(Stack lhs, Action<T> rhs)
            {
                lhs.Add(rhs);

                return lhs;
            }

            public static Stack operator +(Stack lhs, Action<T,T> rhs)
            {
                lhs.Add(rhs);

                return lhs;
            }

        }

        public T OldValue { get; private set; }
        T value = default;
        public T Value
        {
            get => value;
            set => SetValue(value);
        }

        public bool HasChanged { get; private set; }

        public Stack OnSet = new Stack();
        public Stack OnReset = new Stack();
        public Stack OnChange = new Stack();

        public Toggle(T value = default)
        {
            OldValue = value;
            this.value = value;
        }

        void SetValue(T newValue)
        {
            if (Equals(value, newValue))
                return;

            OldValue = value;
            value = newValue;
            HasChanged = true;

            if (!Equals(value, default))
                OnSet.Call(this);
            else
                OnReset.Call(this);

            OnChange.Call(this);

            HasChanged = false;
        }

        public void Clear()
        {
            OnSet.StackClear();
            OnReset.StackClear();
            OnChange.StackClear();
        }

        public static implicit operator T(Toggle<T> toggle) => toggle != null ? toggle.value : default;
        public static implicit operator bool(Toggle<T> toggle) => toggle != null && toggle.value != default;

        public static void Clear(object holder)
        {
            foreach (var field in holder.GetType().GetFields())
                if (typeof(Toggle<T>).IsAssignableFrom(field.FieldType))
                    (field.GetValue(holder) as Toggle<T>).Clear();
        }
    }
}
