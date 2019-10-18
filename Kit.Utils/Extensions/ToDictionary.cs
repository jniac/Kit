using System;
using System.Collections.Generic;
using System.Linq;

namespace Kit.Utils
{
    public static class ToDictionaryExtensions
    {
        public static IDictionary<T, TValue> ToDictionary<T, TValue>(this IEnumerable<T> items, Func<T, int, TValue> valueDelegate)
        {
            int index = 0;

            return items.ToDictionary(value => value, value => valueDelegate(value, index++));
        }
    }
}
