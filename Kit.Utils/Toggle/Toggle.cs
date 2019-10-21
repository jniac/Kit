using System;
using System.Collections.Generic;

namespace Kit.Utils
{
    public abstract class Toggle
    {
        public abstract void Clear();

        public static void Clear(object holder)
        {
            foreach (var field in holder.GetType().GetFields())
                if (typeof(Toggle).IsAssignableFrom(field.FieldType))
                    (field.GetValue(holder) as Toggle).Clear();
        }
    }
}
