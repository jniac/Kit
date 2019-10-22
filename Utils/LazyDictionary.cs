using System.Collections.Generic;

namespace Kit.Utils
{
    // https://stackoverflow.com/questions/2601477/dictionary-returning-a-default-value-if-the-key-does-not-exist
    /*
     * Do not need to use Add() before using [], ex:
     * 
     * var d = new LazyDictionary<string, float>(float.NaN);
     * d["PI"] = 3.141592653589793 // no error!
     * d["an inexisting key"] // returns float.NaN
     */
    public class LazyDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public TValue defaultValue;

        public LazyDictionary(TValue defaultValue = default)
        {
            this.defaultValue = defaultValue;
        }

        public new TValue this[TKey key]
        {
            get => TryGetValue(key, out TValue t) ? t : defaultValue;
            set
            {
                if (ContainsKey(key))
                {
                    base[key] = value;
                }
                else
                {
                    Add(key, value);
                }
            }
        }

    }
}
