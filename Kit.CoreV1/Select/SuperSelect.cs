using System;
namespace Kit.CoreV1
{
    public class Select<T, TLayerTuple> : Select<T>
    {
        public Select() : base(typeof(TLayerTuple).GetGenericArguments()) { }
    }
}
