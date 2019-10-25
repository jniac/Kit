using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Kit
{
    public partial class Event
    {
        public class Register<TValue, TKey>
        {
            protected Dictionary<TKey, HashSet<TValue>> dict = new Dictionary<TKey, HashSet<TValue>>();

            public int TotalCount { get; private set; }

#if DEBUG
            public List<TValue> all = new List<TValue>();
#endif

            public string InfoAllValues
            {
                get
                {
                    var sb = new StringBuilder();

                    sb.Append($"Register({TotalCount})");

                    int count = 0;
                    foreach (var set in dict.Values)
                        foreach (var listener in set)
                            sb.Append($"\n  {count++}: {listener}");

                    return sb.ToString();
                }
            }

            public Dictionary<TKey, HashSet<TValue>>.KeyCollection Keys { get => dict.Keys; }

            public bool Contains(TValue value, TKey key)
            {
                if (dict.TryGetValue(key, out var values))
                    return values.Contains(value);

                return false;
            }

            public bool Add(TValue value, TKey key)
            {
                if (dict.TryGetValue(key, out var values))
                {
                    if (values.Add(value))
                    {
#if DEBUG
                        all.Add(value);
#endif
                        TotalCount++;
                        return true;
                    }

                    return false;
                }

                values = new HashSet<TValue> { value };
                dict.Add(key, values);

#if DEBUG
                all.Add(value);
#endif
                TotalCount++;
                return true;
            }

            public HashSet<TValue> Get(TKey key)
            {
                if (!dict.TryGetValue(key, out var values))
                    values = new HashSet<TValue>();

                return values;
            }

            public bool Remove(TValue value, TKey key)
            {
                if (dict.TryGetValue(key, out var values))
                {
                    bool removed = values.Remove(value);

                    if (removed)
                    {
#if DEBUG
                        all.Remove(value);
#endif
                        TotalCount--;
                    }

                    if (values.Count == 0)
                        dict.Remove(key);

                    return removed;
                }

                return false;
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
