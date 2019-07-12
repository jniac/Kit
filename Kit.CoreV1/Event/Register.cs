using System;
using System.Linq;
using System.Collections.Generic;

namespace Kit.CoreV1
{
    public partial class Event
    {
        public class Register<TValue, TKey>
        {
            protected Dictionary<TKey, HashSet<TValue>> dict = new Dictionary<TKey, HashSet<TValue>>();

            public int GetTotalCount()
            {
                int count = 0;

                foreach (var set in dict.Values)
                    count += set.Count;

                return count;
            }
            public int TotalCount { get => GetTotalCount(); }

            public string GetInfo()
            {
                var strings = new List<string> {  $"Register({TotalCount})" };

                int count = 0;
                foreach (var set in dict.Values)
                    foreach (var listener in set)
                        strings.Add($"\n  {count++}: {listener}");

                return string.Concat(strings);
            }
            public string Info { get => GetInfo(); }

            public void Add(TValue value, TKey key)
            {
                if (dict.TryGetValue(key, out var values))
                {
                    values.Add(value);
                }
                else
                {
                    values = new HashSet<TValue> { value };
                    dict.Add(key, values);
                }
            }

            public HashSet<TValue> Get(TKey key)
            {
                if (!dict.TryGetValue(key, out var values))
                    values = new HashSet<TValue>();

                return values;
            }

            public void Remove(TValue value, TKey key)
            {
                if (dict.TryGetValue(key, out var values))
                {
                    values.Remove(value);

                    if (values.Count == 0)
                        dict.Remove(key);
                }
            }
        }

        public class RegisterWithOptionalKey<TValue, TKey, TOptKey> : Register<TValue, TKey>
        {
            Register<TValue, TOptKey> optionalRegister = new Register<TValue, TOptKey>();

            public void Add(TValue value, TKey key, TOptKey optKey)
            {
                Add(value, key);

                if (optKey != default)
                    optionalRegister.Add(value, optKey);
            }

            public HashSet<TValue> GetWithOptionalKey(TOptKey optKey) => optionalRegister.Get(optKey);

            public void Remove(TValue value, TKey key, TOptKey optKey)
            {
                Remove(value, key);

                if (optKey != default)
                    optionalRegister.Remove(value, optKey);
            }

        }

    }
}
