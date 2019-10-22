using System;
using System.Collections.Generic;
using System.Linq;

namespace Kit.Utils
{
    public static class ToListExtensions
    {
        public static IList<TValue> ToList<T, TValue>(this IEnumerable<T> items, Func<T, TValue> valueDelegate)
        {
            return items.Select(value => valueDelegate(value)).ToList();
        }

        public static IList<TValue> ToList<T, TValue>(this IEnumerable<T> items, Func<T, int, TValue> valueDelegate)
        {
            int index = 0;

            return items.Select(value => valueDelegate(value, index++)).ToList();
        }
    }
}
