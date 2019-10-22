using System;
namespace Kit
{
    public class Select<T, TLayerTuple> : Select<T>
    {
        public Select() : base(typeof(TLayerTuple).GetGenericArguments()) { }
    }
}
