# Roadmap

## DisposableWhile

`DisposableWhile` is better than `While`.  
`DisposableWhile` must replace `While`.  

### More convenient:
no need to embed `On<T>` declaration as arguments.
```csharp
using(While<Init>(target))
using(While<Start>(target))
{
	// do stuff
	On<Yolo>(...);
}
```

```csharp
While<Init>(target,
	While<Start>(target,
		On<Yolo>()));
```

### Compatible with `Select<T>`

```csharp
using (select.While<Active>(target))
{
	On<Yolo>(...);
}
```
