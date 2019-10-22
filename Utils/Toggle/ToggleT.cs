using System;
using System.Collections.Generic;


namespace Kit
{
    public class Toggle<T> : Toggle
    {
        public class Stack
        {
            List<Action> actions;
            List<Action<T>> actionsT;
            List<Action<T, T>> actionsTT;
            List<Action<Toggle<T>>> actionsToggle;

            public void Add(Action action) =>
                (actions ?? (actions = new List<Action>())).Add(action);

            public void Add(Action<T> action) =>
                (actionsT ?? (actionsT = new List<Action<T>>())).Add(action);

            public void Add(Action<T, T> action) =>
                (actionsTT ?? (actionsTT = new List<Action<T, T>>())).Add(action);

            public void Add(Action<Toggle<T>> action) =>
                (actionsToggle ?? (actionsToggle = new List<Action<Toggle<T>>>())).Add(action);

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

                if (actionsToggle != null)
                    foreach (var action in actionsToggle)
                        action(toggle);
            }

            public void StackClear()
            {
                actions?.Clear();
                actionsT?.Clear();
                actionsTT?.Clear();
                actionsToggle?.Clear();

                actions = null;
                actionsT = null;
                actionsTT = null;
                actionsToggle = null;
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

            public static Stack operator +(Stack lhs, Action<T, T> rhs)
            {
                lhs.Add(rhs);

                return lhs;
            }

            public static Stack operator +(Stack lhs, Action<Toggle<T>> rhs)
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

        public Toggle(T value = default, T oldValue = default)
        {
            OldValue = oldValue;
            this.value = value;
        }

        public void ToggleValue()
        {
            SetValue(OldValue);
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

        public override void Clear()
        {
            OnSet.StackClear();
            OnReset.StackClear();
            OnChange.StackClear();
        }

        public static implicit operator T(Toggle<T> toggle) => toggle != null ? toggle.value : default;

        // Generic Clear() useful ? 
        // hm, not sure
        //public static new void Clear(object holder)
        //{
        //    foreach (var field in holder.GetType().GetFields())
        //        if (typeof(Toggle<T>).IsAssignableFrom(field.FieldType))
        //            (field.GetValue(holder) as Toggle<T>).Clear();
        //}
    }
}
