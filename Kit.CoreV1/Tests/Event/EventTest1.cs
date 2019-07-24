using System;
using System.Linq;

namespace Kit.CoreV1.Tests
{
    public static partial class EventTest
    {
        public static class Test1 {
            
            public class MainEvent : Event 
            {
                public class Step : MainEvent { }
                public class Start : MainEvent { }
                public class Stop : MainEvent { }
            }

            public static void Test()
            {
                Console.WriteLine("Test1");

                Node someChild = new Node();
                Node someOtherChild = new Node();
                Node root = new Node().Append(
                    new Node(),
                    new Node().Append(
                        new Node().Append(someChild),
                        new Node()
                    ),
                    new Node().Append(someOtherChild),
                    new Node(),
                    new Node()
                );

                print(root.ToGraphString());
                print();

                Event.On(root, "*", e => print(e));

                Event.Dispatch(new Event
                {
                    Target = root,
                    Type = "hello",
                });

                Event.On<Event<Node>>(root, "*", e => print($"Root reached! ({e.Target} > {e.CurrentTarget})"));
                Event.Dispatch(new Event<Node>
                {
                    Target = someChild,
                    Propagation = t => t.parent,
                });

                print();
                print("Cancel & Once test");
                Event.Once<Event<Node>>(Node.Get(4), "*", e => {
                    print($"Cancel event: ${e}");
                    e.Consume();
                });
                Event.Dispatch(new Event<Node>
                {
                    Target = someChild,
                    Propagation = t => t.parent,
                });
                Event.Dispatch(new Event<Node>
                {
                    Target = someChild,
                    Propagation = t => t.parent,
                });

                print();
                print("Off");
                Event.Off(root);
                Event.Dispatch(new Event<Node>
                {
                    Target = someChild,
                    Propagation = t => t.parent,
                });
                print(Event.Listener.Info);


                print();
                print("Down propagation + EndsGlobal:");

                Event.On<Event<Node>>(someChild, "*",
                    e => print($"Child#{e.CurrentTarget.id} reached from root! ({e.Target} > {e.CurrentTarget})"));
                Event.On<Event<Node>>(someOtherChild, "*",
                    e => print($"Child#{e.CurrentTarget.id} reached from root! ({e.Target} > {e.CurrentTarget})"));
                Event.On<Event<Node>>(e => print($"end of event#{e.id}\n"));

                Event.Dispatch(new Event<Node>
                {
                    Target = root,
                    Propagation = t => t.Children,
                    EndsGlobal = true,
                });

                Event.Once<Event<Node>>(Node.Get(4), "*", e => {
                    print($"Cancel down propagation on event: {e}");
                    e.Consume();
                });

                Event.Dispatch(new Event<Node>
                {
                    Target = root,
                    Propagation = t => t.Children,
                    EndsGlobal = true,
                });

                Event.Dispatch(new Event<Node>
                {
                    Target = root,
                    Propagation = t => t.Children,
                    EndsGlobal = true,
                });

                print();
                print("Phase");

                Event.On<MainEvent.Step>(
                    enter: e => print("enter", e),
                    exit: e => print("exit", e));

                Event.Dispatch(new MainEvent.Step { Enter = true });
                Event.Dispatch(new MainEvent.Step { Exit = true });
            }

        }
    }
}
