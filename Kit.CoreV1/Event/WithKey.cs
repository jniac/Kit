using System;
namespace Kit.CoreV1
{
    public partial class Event
    {
        static object defaultKey;

        public static void WithKey(object key, Action callback)
        {
            object previousDefaultKey = defaultKey;
            defaultKey = key;

            callback();

            defaultKey = previousDefaultKey;
        }
    }
}
