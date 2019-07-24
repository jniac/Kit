using System;
using System.Threading.Tasks;

namespace Kit.CoreV1
{
    public partial class Event
    {
        static int sleepDuration = 10;

        public static async Task<T> Wait<T>(object target = null, object type = null)
            where T : class, IEvent
        {
            T e = null;

            Once<T>(target, type, e2 => e = e2);

            await Task.Run(() =>
            {
                while (e == null)
                    System.Threading.Thread.Sleep(sleepDuration);
            });

            return e;
        }
    }
}
