using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Kit.Utils
{
    // inspired from https://devblogs.microsoft.com/pfxteam/await-anything/
    public static class Await
    {
        public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan) =>
            Task.Delay(timeSpan).GetAwaiter();

        public static TaskAwaiter GetAwaiter(this float seconds) => 
            TimeSpan.FromSeconds(seconds).GetAwaiter();
    }
}
