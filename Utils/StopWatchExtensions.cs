using System;
using System.Diagnostics;

namespace Kit
{
    public static class StopWatchExtensions
    {
        public static int GetElapsedMilliSeconds(this Stopwatch stopWatch)
            => (int)(1e3f * stopWatch.ElapsedTicks / Stopwatch.Frequency);

        public static int GetElapsedMicroSeconds(this Stopwatch stopWatch)
            => (int)(1e6f * stopWatch.ElapsedTicks / Stopwatch.Frequency);

        public static int GetElapsedNanoSeconds(this Stopwatch stopWatch)
            => (int)(1e9f * stopWatch.ElapsedTicks / Stopwatch.Frequency);
    }
}
