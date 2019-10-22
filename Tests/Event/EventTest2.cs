using System.Linq;

namespace Kit.Tests
{
    public static partial class EventTest
    {
        public static class Test2
        {
            public class RunEvent : Event { }

            public class TickEvent : Event
            {
                public int count;
            }

            public static void Test()
            {
                print("Test2");

                print();
                print("WithKey");

                Event.WithKey("yolo", () => {

                    Event.On("foo", "bar", e => print(e));
                    Event.On("foo", "baz", e => print(e));
                    Event.On("foo", "qux", e => print(e));

                });

                print(Event.Listener.InfoAllListener);

                Event.Dispatch(new Event { Target = "foo", Type = "bar" });

                Event.Off(key: "yolo");

                print(Event.Listener.InfoAllListener);

                Event.Dispatch(new Event { Target = "foo", Type = "bar" });



                print();
                print("While");
                Event.While<RunEvent>(
                    Event.On<TickEvent>(e => print($"#1 tick:{e.count}")),
                    Event.On<TickEvent>(e => print($"#2 tick:{e.count}")));

                Event.Dispatch(new TickEvent { count = 0 });

                Event.Dispatch(new RunEvent { Enter = true });

                Event.Dispatch(new TickEvent { count = 1 });

                Event.Dispatch(new RunEvent { Exit = true });

                Event.Dispatch(new TickEvent { count = 2 });
            }
        }
    }
}
