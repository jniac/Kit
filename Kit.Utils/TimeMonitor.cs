using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kit.Utils
{
    public static class TimeMonitor
    {
        static Dictionary<object, FloatAverage> dictionary = new Dictionary<object, FloatAverage>();
        static Stopwatch watch = new Stopwatch();

        public static float Monitor(object key, Action action)
        {
            watch.Restart();

            action();

            watch.Stop();

            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, new FloatAverage());

            FloatAverage floatAverage = dictionary[key];

            floatAverage.CurrentValue = watch.GetElapsedMicroSeconds();

            return floatAverage.CurrentValue;
        }

        public static float GetAverage(object key)
        {
            if (dictionary.TryGetValue(key, out FloatAverage floatAverage))
                return floatAverage.Average;

            return -1;
        }

        public static bool Clear(object key)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                return true;
            }

            return false;
        }
    }
}
