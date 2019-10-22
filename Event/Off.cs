using System;

namespace Kit
{
    public partial class Event
    {
        public static void Off(object target = null, object key = null)
        {
            if (target == null)
            {
                foreach (var listener in Listener.ByKey(key))
                    listener.Destroy(throwIfAlreadyDestroyed: false);
            }
            else
            {
                foreach (var listener in Listener.ByTarget(target))
                    if (key == null || listener.key == key)
                        listener.Destroy(throwIfAlreadyDestroyed: false);
            }
        }
    }
}
