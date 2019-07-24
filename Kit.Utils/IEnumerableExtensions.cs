using System;
using System.Collections.Generic;
using System.Linq;

namespace Kit.Utils
{
    public static class IEnumerableExtensions
    {
        public static string StringJoin<T>(this IEnumerable<T> enumerable, string separator = ", ")
            => string.Join(separator, enumerable.Cast<string>());

        public static (TItem item, TValue value) Extremum<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> minItemTo, bool minimum)
            where TValue : IComparable
        {
            TValue value = default;
            TItem item = default;

            bool first = true;

            foreach (TItem currentItem in items)
            {
                TValue itemValue = minItemTo(currentItem);

                if (first)
                {
                    first = false;
                    value = itemValue;
                    item = currentItem;
                }
                else
                {
                    if (minimum)
                    {
                        if (itemValue.CompareTo(value) < 0)
                        {
                            value = itemValue;
                            item = currentItem;
                        }
                    }
                    else
                    {
                        if (itemValue.CompareTo(value) > 0)
                        {
                            value = itemValue;
                            item = currentItem;
                        }
                    }
                }
            }

            return (item, value);
        }

        public static (TItem item, TValue value) MinItemValue<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> itemToValue)
            where TValue : IComparable
            => Extremum(items, itemToValue, true);

        public static TItem MinItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> itemToValue)
            where TValue : IComparable
            => Extremum(items, itemToValue, true).item;

        public static (TItem item, TValue value) MaxItemValue<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> itemToValue)
            where TValue : IComparable
            => Extremum(items, itemToValue, false);

        public static TItem MaxItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> itemToValue)
            where TValue : IComparable
            => Extremum(items, itemToValue, false).item;
    }
}
