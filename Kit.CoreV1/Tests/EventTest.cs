using System;
using System.Linq;

namespace Kit.CoreV1.Tests
{
    public static partial class EventTest
    {
        static void print(params object[] args) => Console.WriteLine(string.Join(" ", args));

        public static void Test()
        {
            //Test1();
            Test2();
        }
    }
}
