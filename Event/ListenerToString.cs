using System;
using System.Linq;
using System.Collections.Generic;

namespace Kit
{
    public partial class Event
    {
        public partial class Listener
        {
            // Useful for debug.
            public string GetReflectedEventTypeName()
            {
                Type t = eventType;
                string s = eventType.Name;

                while (t.ReflectedType != null)
                {
                    s = t.ReflectedType.Name + "." + s;
                    t = t.ReflectedType;
                }

                return s;
            }

            static string Max(string str, int max) =>
                str.Length < max ? str : str.Substring(0, max - 4) + "...";

            static string Max(object value, int max) =>
                Max(value?.ToString() ?? "", max);

            public string ToFormatA(int keyMaxLength = 16)
            {
                var t = GetReflectedEventTypeName();
                var c = (callback == null ? "__" : "Cb") + (enter == null ? "__" : "En") + (exit == null ? "__" : "Ex");

                return $"{id} {c} {t} k:{Max(key, keyMaxLength)} d:{Destroyed}";
            }

        }
    }
}
