#  Toggle

## Usage:

### Creation
```csharp
public Toggle Active = new Toggle<bool>();
```

### Listener
3 possibilities:
- `Action`
- `Action<T>` where T is the new value
- `Action<T,T>` where (T, T) are (new, old) values

Classic (old fashioned way?):
```csharp
void OnActiveChange1() { }
void OnActiveChange2(bool b) { }

Active.OnChange(OnActiveChange1);
Active.OnChange(OnActiveChange2);
```

Lambda style:
```csharp
Active.OnChange(b => print(b));
Active.OnChange(() => print(Active));
```

### Listener via operator overloading

```csharp
void OnActiveChange1() { }
void OnActiveChange2(bool b) { }

Active += OnActiveChange1;
Active += OnActiveChange2;
```


### Clear

`Toggle.Clear` provide a convenient way to clear (via reflection) all instance of `Toggle` declared as fields on the given object (argument).

```csharp
Toggle.Clear(this)
```
