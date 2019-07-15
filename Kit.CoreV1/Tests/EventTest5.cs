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

                var foo0 = new Foo();
                var foo1 = new Foo();
                var foo2 = new Foo();

                /*
                 * new Select<A, (B, C, D)>();
                 *                 
                 * is equivalent to:
                 * 
                 * new Select<A>(typeof(B), typeof(B), typeof(B));
                 */

                var select = new Select<Foo, (Targeted, Dead)> { foo0, foo1, foo2 };
                print(select.GetLayerInfo());

                Event.On<Targeted>(select, "*",
                    enter: e => print("enter", e.GetType().Name, e.item, e.Phase),
                    exit: e => print("exit ", e.GetType().Name, e.item, e.Phase));

                select.Enter<Targeted>(foo1);
                select.Next<Targeted>(SelectBoundMode.LOOP);
                select.Next<Targeted>(SelectBoundMode.LOOP);

                print();
                print(select.GetLayerInfo());
            }
        }
    }
}
