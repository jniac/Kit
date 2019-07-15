using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Kit.CoreV1.Tests
{
    public static partial class EventTest
    {
        static void print(params object[] args) => Console.WriteLine(string.Join(" ", args));

        public static void RunTest()
        {
            Test5.Test();
        }
    }
}
