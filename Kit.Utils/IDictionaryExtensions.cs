using System;
using System.Collections;
using System.Collections.Generic;

namespace Kit.Utils
{
    public static class IDictionaryExtensions
    {
        public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static IEnumerable<(TKey key, TValue item, int index)> KeyItemIndex<TKey, TValue>
            (this IDictionary<TKey, TValue> dictionary)
        {
            int index = 0;

            foreach (var item in dictionary)
                yield return (item.Key, item.Value, index++);
        }

        public static void Deconstruct(this DictionaryEntry entry, out object key, out object value)
        {
            key = entry.Key;
            value = entry.Value;
        }

    }
}
