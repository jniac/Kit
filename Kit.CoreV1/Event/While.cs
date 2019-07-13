using System.Linq;
using System.Collections.Generic;

namespace Kit.CoreV1
{
    public partial class Event
    {
        public static Listener While<T>(object target, object type, params Listener[] listeners)
            where T : Event
        {
            Listener master = null;

            master = On<T>(target, type,
                enter:e => master.ChildrenDisabled = false, 
                exit:e => master.ChildrenDisabled = true);

            master.ChildrenDisabled = true;

            foreach (var listener in listeners)
                master.AddChild(listener);

            return master;
        }

        public static Listener While<T>(params Listener[] listeners)
            where T : Event
            => While<T>(global, "*", listeners);
    }
}
