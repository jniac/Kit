using System;
using System.Collections.Generic;
using System.Linq;

namespace Kit.Utils
{
    public static class IEnumerableExtensions
    {
        public static string StringJoin<T>(this IEnumerable<T> enumerable, string separator = ", ")
            => string.Join(separator, enumerable.Cast<string>());

        public static (TItem item, TValue value) MinItemValue<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> minItemTo) 
            where TValue : IComparable
        {
            TValue minValue = default;
            TItem minItem = default;

            bool first = true;

            foreach (TItem item in items)
            {
                TValue itemValue = minItemTo(item);

                if (first)
                {
                    first = false;
                    minValue = itemValue;
                    minItem = item;
                }
                else
                {
                    if (itemValue.CompareTo(minValue) < 0)
                    {
                        minValue = itemValue;
                        minItem = item;
                    }
                }
            }

            return (minItem, minValue);
        }

        public static TItem MinItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> minItemTo)
            where TValue : IComparable
            => MinItemValue(items, minItemTo).item;
    }
}
