using System;
using System.Linq;
using System.Collections.Generic;

namespace Kit
{
    public partial class Select<T>
    {
        public Event.DisposableWhile<TLayer> While<TLayer>(T target)
            where TLayer : class, IEvent
        {
            bool childrenDisabled = !DidEnter<TLayer>(target);

            return Event.WhileIDisposable<TLayer>(target, childrenDisabled);
        }
    }
}
