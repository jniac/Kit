using System;
using System.Collections.Generic;

namespace Kit.Utils
{
    public class Toggle
    {
        bool value = false;
        public bool Value
        {
            get => value;
            set => SetValue(value);
        }

        public bool HasChanged { get; private set; }

        List<Action> actions1;
        public List<Action> Actions1 => 
            actions1 ?? (actions1 = new List<Action>());

        List<Action<bool>> actions2;
        public List<Action<bool>> Actions2 => 
            actions2 ?? (actions2 = new List<Action<bool>>());

        public Toggle(bool value = false)
        {
            this.value = value;
        }

        void SetValue(bool newValue)
        {
            if (value == newValue)
                return;

            value = newValue;
            HasChanged = true;

            if (actions1 != null)
                foreach (var action in actions1)
                    action();

            if (actions2 != null)
                foreach (var action in actions2)
                    action(value);

            HasChanged = false;
        }

        void Clear()
        {
            actions1?.Clear();
            actions2?.Clear();

            actions1 = null;
            actions2 = null;
        }

        public void OnChange(Action action) =>
            Actions1.Add(action);

        public void OnChange(Action<bool> action) =>
            Actions2.Add(action);

        public static Toggle operator +(Toggle lhs, Action rhs)
        {
            lhs.OnChange(rhs);

            return lhs;
        }

        public static Toggle operator +(Toggle lhs, Action<bool> rhs)
        {
            lhs.OnChange(rhs);

            return lhs;
        }

        public static implicit operator bool(Toggle toggle) => toggle != null && toggle.value;

        public static void Clear(object holder)
        {
            foreach (var field in holder.GetType().GetFields())
                if (typeof(Toggle).IsAssignableFrom(field.FieldType))
                    (field.GetValue(holder) as Toggle).Clear();
        }
    }
}
