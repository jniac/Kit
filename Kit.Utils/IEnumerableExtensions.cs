using System;
using System.Collections.Generic;
using System.Linq;

namespace Kit.Utils
{
    public static class IEnumerableExtensions
    {
        public static string StringJoin<T>(this IEnumerable<T> enumerable, string separator = ", ")
            => string.Join(separator, enumerable.Select(item => item?.ToString()));

        public static string StringInfo<T>(this IEnumerable<T> items, string separator = ", ")
            => $"{items.GetType().Name}<{typeof(T).Name}>({items.Count()}) {StringJoin(items, separator)}";

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
            Action<(T itemA, T itemB, int index, int total)> callback, bool closed = false)
        {
            int count = 0;
            int total = items.Count() + (closed ? 0 : -1);
            T previous = default, first = default;

            foreach (T item in items)
            {
                if (count == 0)
                    first = item;
                else
                    callback((previous, item, count - 1, total));

                previous = item;
                count++;
            }

            if (closed)
                callback((previous, first, count - 1, total));

            return items;
        }
        public static IEnumerable<T> ForInterval<T>(this IEnumerable<T> items,
            Action<T, T> callback, bool closed = false)
            => ForInterval(items, i => callback(i.itemA, i.itemB), closed);

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


        /// <summary>
        /// Distribute the specified items into two (or more) lists, according specified conditions.
        /// </summary>
        /// <returns>The lists.</returns>
        /// <param name="items">Items.</param>
        /// <param name="condition1">Condition1.</param>
        /// <param name="condition2">Condition2.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static (List<T> list1, List<T> list2) Distribute<T>(this IEnumerable<T> items, Func<T, bool> condition1, Func<T, bool> condition2)
        {
            var list1 = new List<T>();
            var list2 = new List<T>();

            foreach(T item in items)
            {
                if (condition1(item))
                    list1.Add(item);

                if (condition2(item))
                    list2.Add(item);
            }

            return (list1, list2);
        }
        public static (List<T> list1, List<T> list2) Distribute2<T>(this IEnumerable<T> items, Func<T, int> distribution)
        {
            var list1 = new List<T>();
            var list2 = new List<T>();

            foreach (T item in items)
            {
                switch (distribution(item))
                {
                    case 1:
                        list1.Add(item);
                        break;

                    case 2:
                        list2.Add(item);
                        break;
                }
            }

            return (list1, list2);
        }

        public static (List<T> list1, List<T> list2, List<T> list3) 
            Distribute3<T>(this IEnumerable<T> items, Func<T, int> distribution)
        {
            var list1 = new List<T>();
            var list2 = new List<T>();
            var list3 = new List<T>();

            foreach (T item in items)
            {
                switch (distribution(item))
                {
                    case 1:
                        list1.Add(item);
                        break;

                    case 2:
                        list2.Add(item);
                        break;

                    case 3:
                        list3.Add(item);
                        break;
                }
            }

            return (list1, list2, list3);
        }
    }
}
