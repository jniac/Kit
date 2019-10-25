using System;

namespace Kit
{
	// NOTE: hm, not clear actually,
	// should IEvent become the base of dispatching with some GetTarget() method to implement ?
    public interface IEvent { }
    public interface IConsumableEvent { }
    public interface IEndsGlobalEvent { }
    public interface IStartsGlobalEvent { }

    // Design
    public interface IInstantaneous { } // instantaneous cannot have Enter or Exit phases 
}
