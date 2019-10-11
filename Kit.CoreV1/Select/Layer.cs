using System;
using System.Collections.Generic;
using System.Linq;

namespace Kit.CoreV1
{
    public enum SelectLayerMode
    {
        Single,
        Multiple,
    }

    public partial class Select<T>
    {
        public class Layer
        {
            public readonly object key;
            public readonly Select<T> select;

            public SelectLayerMode layerMode = SelectLayerMode.Single;
            public SelectBoundMode boundMode = SelectBoundMode.CLAMP;

            public HashSet<T> set = new HashSet<T>();
            List<Event> events = new List<Event>();

            public int Count => set.Count;

            public Func<Layer, string> ExtractName = 
                v => (v.key is Type) ? (v.key as Type).Name : v.key.ToString();
            public string GetName() => ExtractName(this);

            public Layer(object key, Select<T> select)
            {
                this.key = key;
                this.select = select;
            }

            public bool DidEnter(T item) => set.Contains(item);

            void CreateSelectedEvent(Type eventType, T item, EventPhase phase)
            {
                Event e = (Event)Activator.CreateInstance(eventType);

                e.Targets = new object[] { item, select };
                e.Type = key;
                e.Phase = phase;

                if (e is SelectEvent<T> se)
                {
                    se.select = select;
                    se.layer = this;
                    se.item = item;
                }

                events.Add(e);
            }

            void DoChange()
            {
                if (events.Count > 0)
                {
                    events.Add(new SelectEvent<T>.Change
                    {
                        Target = select,
                        Type = key,
                        select = select,
                        layer = this,
                    });

                    foreach (var e in events)
                        Event.Dispatch(e);

                    events.Clear();
                }
            }

            public T GetCurrentItem()
            {
                return set.FirstOrDefault();
            }

            public T[] GetCurrentItems()
            {
                return set.ToArray();
            }

            void DoEnter(T item)
            {
                // NOTE: this can be optimized with an internal hashset.
                if (!select.Contains(item))
                    select.Add(item);

                set.Add(item);

                CreateSelectedEvent(typeof(SelectEvent<T>.Selected), item, EventPhase.ENTER);

                //if (key is Type && typeof(SelectEvent<T>).IsAssignableFrom(key as Type))
                if (key is Type && typeof(Event).IsAssignableFrom(key as Type))
                    CreateSelectedEvent(key as Type, item, EventPhase.ENTER);         
            }

            public void Enter(T item)
            {
                if (set.Contains(item))
                    return;

                if (layerMode == SelectLayerMode.Single && set.Count == 1)
                    DoExit(set.First());

                DoEnter(item);

                DoChange();
            }

            public void EnterAll(IEnumerable<T> items)
            {
                if (!items.Any())
                    return;

                // NOTE: if 'single', no more error here, instead enter only the last item.
                if (layerMode == SelectLayerMode.Single)
                {
                    Enter(items.Last());
                }
                else
                {
                    foreach (T item in items)
                        if (!set.Contains(item))
                            DoEnter(item);

                    DoChange();
                }
            }

            public void EnterAll() => EnterAll(select);

            public void Enter(params T[] items) => EnterAll(items);



            void DoExit(T item)
            {
                set.Remove(item);

                if (select.AutoRemoveItem 
                    && !select.layers.Values.Any(layer => layer.set.Contains(item)))
                        select.Remove(item);

                if (select.AutoRemoveLayer 
                    && set.Count == 0)
                    select.layers.Remove(key);

                CreateSelectedEvent(typeof(SelectEvent<T>.Selected), item, EventPhase.EXIT);

                //if (key is Type && typeof(SelectEvent<T>).IsAssignableFrom(key as Type))
                if (key is Type && typeof(Event).IsAssignableFrom(key as Type))
                    CreateSelectedEvent(key as Type, item, EventPhase.EXIT);
            }

            public void Exit(T item)
            {
                if (set.Contains(item))
                    DoExit(item);

                DoChange();
            }

            public void ExitAll(IEnumerable<T> items)
            {
                foreach (T item in items)
                    if (set.Contains(item))
                        DoExit(item);

                DoChange();
            }

            public void ExitAll() => ExitAll(set.ToArray());

            public void Exit(params T[] items) => ExitAll(items);



            public void Toggle(bool enter, T item)
            {
                if (enter)
                {
                    Enter(item);
                }
                else
                {
                    Exit(item);
                }
            }


            void Increment(SelectBoundMode incrementBoundMode, int step)
            {
                T currentItem = set.FirstOrDefault();
                int currentIndex = select.list.IndexOf(currentItem);

                if (currentIndex != -1)
                    DoExit(currentItem);

                int nextIndex = Bounded(currentIndex + 1, select.Count, incrementBoundMode);
                T nextItem = select.list[nextIndex];

                if (nextIndex != -1)
                    DoEnter(nextItem);

                DoChange();
            }

            public void Next()
                => Increment(boundMode, 1);

            public void Previous()
                => Increment(boundMode, -1);

            public void Next(SelectBoundMode boundMode)
                => Increment(boundMode, 1);

            public void Previous(SelectBoundMode boundMode)
                => Increment(boundMode, -1);

            public string GetInfo() => $"{key}({set.Count}): {string.Join(",", set)}";

            public string GetDetailedInfo() =>
                $"{key}({set.Count}):\n{string.Join(",", set.Select(v => $"#{select.list.IndexOf(v)}"))}";
        }
    }
}
