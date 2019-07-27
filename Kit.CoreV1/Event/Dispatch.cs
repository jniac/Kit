using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Kit.CoreV1
{
    public partial class Event
    {
        object[] GetPropagationTargets(object t)
        {
            if (t == null || Equals(t, global))
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

            if (e.target != null)
                head.Enqueue(e.target);

            if (e.targets != null)
                foreach (object t in e.targets)
                    if (t != null)
                        head.Enqueue(t);

            while (head.Count > 0)
            {
                object currentTarget = head.Dequeue();
                object[] newTargets = e.GetPropagationTargets(currentTarget);

                tree.Add(currentTarget, newTargets);

                foreach (var newTarget in newTargets)
                    head.Enqueue(newTarget);
            }

            return tree;
        }

        public static bool Dispatching { get; private set; } = false;
        static List<Action> afterDispatching = new List<Action>();

        static void DoDispatch(Event e)
        {
            if (e.OnDispatch != null)
                afterDispatching.Add(e.OnDispatch);

            var tree = GetTree(e);

            // 1. Collect

            Listener[] startListeners =
                e is IStartsGlobalEvent || e.StartsGlobal ?
                Listener.Get(global, e) : null;

            Listener[] endListeners =
                e is IEndsGlobalEvent || e.EndsGlobal ?
                Listener.Get(global, e) : null;

            var treeListeners = new Dictionary<object, Listener[]>();
            foreach (object target in tree.Keys)
                treeListeners.Add(target, Listener.Get(target, e));

            // 2. Invoke

            e.Locked = true;

            if (startListeners != null)
                foreach (Listener listener in startListeners)
                    listener.Invoke(e);

            var head = new Queue<object>();

            if (e.target != null)
                head.Enqueue(e.target);

            if (e.targets != null)
                foreach (object t in e.targets)
                    if (t != null)
                        head.Enqueue(t);

            while (head.Count > 0)
            {
                object currentTarget = head.Dequeue();

                e.Consumed = false;
                e.currentTarget = currentTarget;

                foreach (Listener listener in treeListeners[currentTarget])
                {
                    listener.Invoke(e);

                    if (e.Consumed)
                        break;
                }

                if (!e.Consumed)
                    foreach (var newTarget in tree[currentTarget])
                        head.Enqueue(newTarget);
            }

            if (!e.Consumed && endListeners != null)
                foreach (Listener listener in endListeners)
                    listener.Invoke(e);
        }

        public static void Dispatch(Event e)
        {
            if (Dispatching)
            {
                afterDispatching.Add(() => Dispatch(e));
                return;
            }

            Dispatching = true;

            DoDispatch(e);

            Dispatching = false;

            Action[] actions = afterDispatching.ToArray();
            afterDispatching.Clear();

            foreach (var action in actions)
                action();
        }
    }
}
