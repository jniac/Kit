using System;
using System.Threading.Tasks;

namespace Kit
{
    public partial class Event
    {
        // classic
        static Action<Event> ToAction(Action action)
            => action == null ? null : (Action<Event>)(e => action());
        static Action<Event> ToAction(Func<Event, Task> action)
            => action == null ? null : (Action<Event>)(e => action(e));
        static Action<Event> ToAction(Func<Task> action)
            => action == null ? null : (Action<Event>)(e => action());

        // generic
        static Action<T> ToAction<T>(Action action)
            => action == null ? null : (Action<T>)(e => action());
        static Action<T> ToAction<T>(Func<T, Task> action)
            => action == null ? null : (Action<T>)(e => action(e));
        public static Action<T> ToAction<T>(Func<Task> action)
            => action == null ? null : (Action<T>)(e => action());
    }
}
