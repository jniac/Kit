using System;

namespace Kit.CoreV1.Tests
{
    public static partial class EventTest
    {
        public static class Test5
        {
            class Targeted : SelectEvent<Foo> { }
            class Dead : SelectEvent<Foo> { }

            public static void Test()
            {
                print("Test5");

                print();
                print("Select Test with events as layers");
                print();

                var select = new Select<Foo> { foo0, foo1, foo2 };
                print(select.GetInfo());
                print();

                Event.On<Targeted>(select, "*",
                    enter: e => print("enter", e.GetType().Name, e.item, e.Phase),
                    exit: e => print("exit ", e.GetType().Name, e.item, e.Phase));

                select.Enter<Targeted>(foo1);
                select.Next<Targeted>(SelectBoundMode.LOOP);
                select.Next<Targeted>(SelectBoundMode.LOOP);

                print();
                print(select.GetInfo());

                select.Enter<Dead>(foo1);

                print();
                print(select.GetInfo());
            }
        }
    }
}
