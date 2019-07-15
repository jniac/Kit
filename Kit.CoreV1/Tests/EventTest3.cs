using System.Linq;

namespace Kit.CoreV1.Tests
{
    public partial class EventTest
    {
        class Foo
        {
            static int count;
            public readonly int id = count++;
            public override string ToString() => $"Foo#{id}";
        }

        enum FooState
        {
            SelectedByPlayer,
            Targeted,
            Dead,
        }

        public static void Test3()
        {
            print("Test3");

            print();
            print("Select Test");

            var foo1 = new Foo();
            var foo2 = new Foo();
            var foo3 = new Foo();
            var foo4 = new Foo();

            var select = new Select<Foo>("FooSelect") { foo1, foo2, foo3 };

            Event.On<SelectEvent<Foo>.Selected>(select, "*", e
                => print(e.GetType().Name.PadRight(10), e.item, e.Type, e.Phase));

            Event.On<SelectEvent<Foo>.Change>(select, "*", e
                => print(e.GetType().Name.PadRight(10), e.Type, string.Join<object>(",", e.select.GetLayer(e.Type).GetCurrentItems())));

            select.Enter(foo1);
            select.Enter(FooState.SelectedByPlayer, foo1);
            select.Enter(FooState.SelectedByPlayer, foo2);

            select.GetLayer(FooState.Targeted).mode = SelectLayerMode.Multiple;
            select.Enter(FooState.Targeted, foo1);
            select.Enter(FooState.Targeted, foo2, foo3);

            print();
            print(select.GetInfo());

            select.Remove(foo3);

            print();
            print(select.GetInfo());

            select.Add(foo4);
            select.Enter(FooState.Targeted, foo2, foo3);
        }

        public static void Test4()
        {
            print("Test4");

            print();
            print("Select Test with Enum");

            var foo1 = new Foo();
            var foo2 = new Foo();
            var foo3 = new Foo();
            var foo4 = new Foo();

            var select = new Select<Foo>(typeof(FooState)) { foo1, foo2, foo3 };

            select.GetLayer(FooState.SelectedByPlayer).mode = SelectLayerMode.Multiple;

            select.Enter(foo1);
            select.Enter(FooState.SelectedByPlayer, foo2);
            select.Enter(FooState.Dead, foo3);

            select.EnterAll();

            print();
            print(select.GetInfo());
        }
    }
}
