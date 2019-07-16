using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Kit.CoreV1.Tests
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

        public static void RunTest()
        {
            Test5.Test();
        }
    }
}
