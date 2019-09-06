using System;
using System.Collections.Generic;

namespace Kit.Utils
{
    public static class IListExtensions
    {
        public static T PopAt<T>(this IList<T> list, int index, bool allowBackward = true)
        {
            int count = list.Count;
            int i = index < 0 && allowBackward ? count + index : index;

            if (i < 0 || i >= count)
                throw new Exception($"oups, the current IList has no item to pop (index: {i}, Count: {count})");

            T item = list[i];
            list.RemoveAt(i);

            return item;
        }
        public static T PopFirst<T>(this IList<T> list) =>
            PopAt(list, 0);

        public static T PopLast<T>(this IList<T> list) =>
            PopAt(list, -1, true);


        /*
         * for conditional test, eg:
         * if ([a, b, c, d].TryGetValue(whateverIndex, out T item))
         *      somethingWithItem(item)
         */
        public static bool TryGetValue<T>(this IList<T> list, int index, out T value)
        {
            if (index < 0 || index >= list.Count)
            {
                value = default;
                return false;
            }

            value = list[index];
            return true;
        }
    }
}
