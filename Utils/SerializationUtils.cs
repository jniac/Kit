using System;
using System.Collections;
using System.Linq;

namespace Kit
{
    public static class SerializationUtils
    {
        public static string ValueToString(object value)
        {
            if (value == null)
                return "null";

            if (value is string str)
                return str;

            if (value is IEnumerable enumerable)
                return enumerable.Cast<object>().Select(v => $"\n- {v}").StringJoin("");

            return value.ToString();
        }

        public static string ObjectToString(object obj)
        {
            if (obj == null)
                return "null";

            Type type = obj.GetType();

            if (type.IsValueType)
                return obj.ToString();

            var fields = type.GetFields()
                .Where(f => f.IsPublic && !f.IsStatic)
                .Select(f => $"{f.Name}: {ValueToString(f.GetValue(obj))}")
                .ToArray();

            var properties = type.GetProperties()
                .Where(p => p.CanRead && !p.IsStatic())
                .Select(p => $"{p.Name}: {ValueToString(p.GetValue(obj))}")
                .ToArray();

            return $"{type.Name} instance:\n" +
                string.Join("\n", fields) + "\n" +
                string.Join("\n", properties);
        }
    }
}
