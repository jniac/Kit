using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kit.CoreV1
{
    public partial class Event
    {
        public partial class Listener
        {
            protected List<InvocationAwaiter> awaiters = new List<InvocationAwaiter>();

            protected void DisposeAwaiters(Event e)
            {
                var array = awaiters.ToArray();

                awaiters.Clear();

                foreach (InvocationAwaiter awaiter in array)
                    awaiter.Invoke(e);
            }

            public struct InvocationAwaitable
            {
                readonly Listener listener;

                public InvocationAwaitable(Listener listener) =>
                    this.listener = listener;

                public InvocationAwaiter GetAwaiter() =>
                    new InvocationAwaiter(listener);
            }

            public class InvocationAwaiter : INotifyCompletion
            {
                readonly Listener listener;

                public InvocationAwaiter(Listener listener)
                {
                    this.listener = listener;
                    listener.awaiters.Add(this);
                }

                Action continuation;

                protected Event e;

                public bool IsCompleted { get; private set; } = false;

                public Event GetResult() => e;

                public void OnCompleted(Action continuation) =>
                    this.continuation = continuation;

                public void Invoke(Event e)
                {
                    IsCompleted = true;
                    this.e = e;

                    continuation?.Invoke();
                }
            }

            public InvocationAwaitable Invocation { get => new InvocationAwaitable(this); }
        }

        // NOTE: Since generic Listener<T> is not required anywhere except here, 
        // here is defined the class, but definition could become "partial" later.
        public class Listener<T> : Listener
            where T : class, IEvent
        {
            public Listener(object target, object type, object key,
                Action<Event> callback,
                Action<Event> enter,
                Action<Event> exit,
                Type eventType,
                int priority,
                bool consume) : base(target, type, key, callback, enter, exit, eventType, priority, consume) { }

            public new struct InvocationAwaitable
            {
                readonly Listener<T> listener;

                public InvocationAwaitable(Listener<T> listener) =>
                    this.listener = listener;

                public InvocationAwaiterT GetAwaiter()
                    => new InvocationAwaiterT(listener);
            }

            public class InvocationAwaiterT : InvocationAwaiter
            {
                public InvocationAwaiterT(Listener listener) : base(listener) { }

                public new T GetResult() => e as T;
            }

            public new InvocationAwaitable Invocation { get => new InvocationAwaitable(this); }
        }
    }
}
