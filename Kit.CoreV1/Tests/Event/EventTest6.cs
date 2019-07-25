using System;

namespace Kit.CoreV1.Tests
{
    public static partial class EventTest
    {
        public static class Test6
        {
            public static void Test()
            {
                print("Test6");

                print();
                print("Test priority");
                print();

                Event.On(Event.global, "tap", e => print($"0 PRIORITY_LOW {e}"), priority: Event.PRIORITY_LOW);
                Event.On(Event.global, "tap", e => print($"1 {e}"));
                Event.On(Event.global, "tap", e => print($"2 {e}"));
                Event.On(Event.global, "tap", e => print($"3 PRIORITY_HIGH {e}"), priority: Event.PRIORITY_HIGH);
                Event.On(Event.global, "tap", e => print($"4 {e}"));
                Event.On(Event.global, "tap", e => print($"5 {e}"));

                Event.Dispatch(new Event { Type = "tap" });
            }
        }
    }
}
