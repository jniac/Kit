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




        /*
         * Interval loop, eg:
         * [a, b, c, d].ForInterval(interval => {
         *      var (item0, item1, index, total) = interval;       
         * })
         */
        public static IEnumerable<T> ForInterval<T>(this IEnumerable<T> items,
            Action<(T itemA, T itemB, int index, int total)> callback)
        {
            int count = 0;
            int total = items.Count() - 1;
            T previous = default;

            foreach (T item in items)
            {
                if (count > 0)
                    callback((previous, item, count - 1, total));

                previous = item;
                count++;
            }

            return items;
        }
        public static IEnumerable<T> ForInterval<T>(this IEnumerable<T> items,
            Action<T, T> callback)
            => ForInterval(items, i => callback(i.itemA, i.itemB));

        /*
         * useful for distance calculation, eg:
         * total = [a, b, c, d].ReduceInterval((d, p0, p1) => d + distance(p1 - p0), 0f)
         */
        public static U ReduceInterval<T, U>(this IEnumerable<T> items, Func<U, T, T, U> callback, U start = default)
        {
            items.ForInterval((a, b) => start = callback(start, a, b));

            return start;
        }
    
        /// <summary>
        /// Get Item AND index from any IEnumerable instance.
        /// </summary>
        /// <returns>ValueTuple (T item, int index)</returns>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<(T item, int index)> ItemIndex<T>(this IEnumerable<T> list)
        {
            int index = 0;

            foreach (T item in list)
                yield return (item, index++);
        }
    }
}
