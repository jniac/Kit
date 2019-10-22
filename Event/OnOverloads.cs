using System;
using System.Threading.Tasks;

namespace Kit.CoreV1
{
    public partial class Event
    {
        public static Listener On<T>(object target,
            Func<Task> callback,
            object key = null)
            where T : Event
            => On<T>(target, "*", ToAction<T>(callback), key: key);
    }
}
