using System;
using System.Collections.Generic;

namespace Kit.Utils
{
    public static class IListExtensions
    {
        public static T PopFirstItem<T>(this IList<T> list)
        {
            if (list.Count == 0)
                throw new Exception("oups, the current IList has no item to pop (Count == 0)");

            T item = list[0];
            list.RemoveAt(0);

            return item;
        }

        public static T PopLastItem<T>(this IList<T> list)
        {
            if (list.Count == 0)
                throw new Exception("oups, the current IList has no item to pop (Count == 0)");

            int index = list.Count - 1;
            T item = list[index];
            list.RemoveAt(index);

            return item;
        }

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
