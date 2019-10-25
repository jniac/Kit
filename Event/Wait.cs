using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Kit
{
    public partial class Event
    {
        public static Listener.InvocationAwaitable Wait(object target = null, object type = null, object key = null, int priority = PRIORITY_NORMAL)
            => Once(target, type, key: key, priority: priority).Invocation;

        public static Listener<T>.InvocationAwaitable Wait<T>(object target = null, object type = null, object key = null, int priority = PRIORITY_NORMAL)
            where T : class, IEvent
            => Once<T>(target, type, key: key, priority: priority).Invocation;
    }
}
