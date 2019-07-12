using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Kit.CoreV1
{
    public partial class Event
    {
        object[] GetTargets(object t)
        {
            if (t.Equals(global))
                return new object[0];

            object newTarget = InvokePropagation(t);

            if (newTarget == null)
                return new object[0];

            if (newTarget is IList)
            {
                var list = new List<object>();

                foreach (object t2 in newTarget as IList)
                    if (t2 != null) // important: remove null targets
                        list.Add(t2);

                return list.ToArray();
            }

            return new object[] { newTarget };
        }

        static Dictionary<object, object[]> GetTree(Event e)
        {
            var tree = new Dictionary<object, object[]>();

            var head = new Queue<object>();
            head.Enqueue(e.target);

            while (head.Count > 0)
            {
                object currentTarget = head.Dequeue();
                object[] newTargets = e.GetTargets(currentTarget);

                tree.Add(currentTarget, newTargets);

                foreach (var newTarget in newTargets)
                    head.Enqueue(newTarget);
            }

            return tree;
        }

        public static void Dispatch(Event e)
        {
            var tree = GetTree(e);

            // 1. Collect

            Listener[] startListeners = e.StartsGlobal ? Listener.Get(global, e).ToArray() : null;
            Listener[] endListeners = e.EndsGlobal ? Listener.Get(global, e).ToArray() : null;

            var treeListeners = new Dictionary<object, Listener[]>();
            foreach (object target in tree.Keys)
                treeListeners.Add(target, Listener.Get(target, e).ToArray());

            // 2. Invoke

            e.Locked = true;

            if (startListeners != null)
                foreach (Listener listener in startListeners)
                    listener.Invoke(e);
                    
            var head = new Queue<object>();
            head.Enqueue(e.target);

            while (head.Count > 0)
            {
                object currentTarget = head.Dequeue();

                e.Canceled = false;
                e.currentTarget = currentTarget;

                foreach (Listener listener in treeListeners[currentTarget])
                {
                    listener.Invoke(e);

                    if (e.Canceled)
                        break;
                }

                if (!e.Canceled)
                    foreach (var newTarget in tree[currentTarget])
                        head.Enqueue(newTarget);
            }

            if (endListeners != null)
                foreach (Listener listener in endListeners)
                    listener.Invoke(e);

        }
    }
}
