using System;
using System.Collections.Generic;

namespace Kit
{
    public static class KeyValuePairExtension
    {
        // ex:
        // Dictionary<string, float> dict = new Dictionary<string, float> { ["One"] = 1, ["Two"] = 2 };
        // foreach (var (key, value) in dict) 
        // { ... }
        public static void Deconstruct<T, U>(this KeyValuePair<T, U> entry, out T key, out U value)
        {
            key = entry.Key;
            value = entry.Value;
        }
    }
}
