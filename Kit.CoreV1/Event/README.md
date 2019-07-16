# Event manifest

Kit.Event provides a versatile tool to handle common event design.

## Overview

### On()

`On()` creates a listener (and returns it):

```c#
Event.On(target, type, e => { ... });
```

### Dispatch()
```c#
Event.Dispatch(new Event
{
    Target = this,
    Type = "Click",
});
```

### Generic Type

Listener/Event matching is based on `(object)e.target`, `(object)e.type` AND `(System.Type)e.GetType()`.  
So, casting a event may be done in generic way:
```c#
Event.On(target, "Click", e => { ... });

// becomes

class Click : Event {
    public Pointer pointer;
}

Event.On<Click>(target, e => {

    // e is Click
    e.pointer.position

});
```

### Phases

Event have a `(EventPhase)Phase` property (enum).  
There are 3 values:
- NONE
- ENTER
- EXIT

Listener can be added on a specific phase:
```c#
Event.On(target, type,
    enter: e => { ... },
    exit: e => { ... });

Event.On<Selected>(target,
    enter: e => { ... },
    exit: e => { ... });
```

### Key & Off()

When creating a Listener, a key may be specified.  
Keys facilitate the removal of listeners:
```c#
Event.On(target, type, e => { ... }, key: this);

Event.Off(key: this);
```
