using System.Linq;

namespace Kit.CoreV1.Tests
{
    public partial class EventTest
    {
        public static void Test2()
        {
            print("Test2");

            print();
            print("WithKey");

            Event.WithKey("yolo", () => {

                Event.On("foo", "bar", e => print(e));
                Event.On("foo", "baz", e => print(e));
                Event.On("foo", "qux", e => print(e));

            });

            print(Event.Listener.Info);

            Event.Dispatch(new Event { Target = "foo", Type = "bar" });

            Event.Off(key: "yolo");

            print(Event.Listener.Info);

            Event.Dispatch(new Event { Target = "foo", Type = "bar" });
        }
    }
}
