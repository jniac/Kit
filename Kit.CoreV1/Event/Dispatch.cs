using System;
using System.Collections;
using System.Collections.Generic;

namespace Kit.CoreV1
{
    public partial class Event
    {
        IEnumerable<object> GetTargets()
        {
            var list = new List<object>();

            if (target != null)
                list.Add(target);

            if (targets != null)
                foreach (object t in targets)
                    if (t != null)
                        list.Add(t);

            return list;
        }

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

            foreach (var target in e.GetTargets())
                head.Enqueue(target);

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
        // NOTE: using Queue instead of List seems more robust when nested Dispatch() are called.
        // [afterDispatching] seems to respect order of incoming dispatch.
        // WARN: This must be handled very carefully since it can cause very serious problems.
        static Queue<Action> afterDispatching = new Queue<Action>();

        static void DoDispatch(Event e)
        {
            if (e.OnDispatch != null)
                afterDispatching.Enqueue(e.OnDispatch);

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
                    if (listener.Invoke(e).Consumed)
                        break;

            var head = new Queue<object>();

            foreach (var target in e.GetTargets())
                head.Enqueue(target);

            while (head.Count > 0)
            {
                object currentTarget = head.Dequeue();

                e.Consumed = false;
                e.currentTarget = currentTarget;

                foreach (Listener listener in treeListeners[currentTarget])
                    if (listener.Invoke(e).Consumed)
                        break;

                if (!e.Consumed)
                    foreach (var newTarget in tree[currentTarget])
                        head.Enqueue(newTarget);
            }

            if (!e.Consumed && endListeners != null)
                foreach (Listener listener in endListeners)
                    if (listener.Invoke(e).Consumed)
                        break;
        }

        public static void Dispatch(Event e)
        {
            if (Dispatching)
            {
                afterDispatching.Enqueue(() => Dispatch(e));
                return;
            }

            Dispatching = true;

            DoDispatch(e);

            Dispatching = false;

            // NOTE: here it was different before (and buggy):
            // Action[] actions = afterDispatching.ToArray();
            // afterDispatching.Clear();
            // then loop en [actions]
            // cf: NOTE & WARN higher
            while (afterDispatching.Count > 0)
                afterDispatching.Dequeue().Invoke();
        }
    }
}
