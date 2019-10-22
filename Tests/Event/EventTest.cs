using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Kit.Tests
{
    public static partial class EventTest
    {
        static void print(params object[] args) => Console.WriteLine(string.Join(" ", args));

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

        static Foo foo0 = new Foo();
        static Foo foo1 = new Foo();
        static Foo foo2 = new Foo();
        static Foo foo3 = new Foo();
        static Foo foo4 = new Foo();

        public static void RunTest()
        {
            Test1.Test();
        }
    }
}
