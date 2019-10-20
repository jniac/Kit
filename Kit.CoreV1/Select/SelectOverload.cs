using System;
using System.Collections.Generic;
using System.Linq;

namespace Kit.CoreV1
{
    public partial class Select<T>
    {
        // Get: Generic
        public IEnumerable<T> Get<TLayer>() =>
            GetLayer<TLayer>().set;

        public T First<TLayer>() =>
            GetLayer<TLayer>().set.First();


        // Enter: layer
        public void Enter(object layerKey, T item)
            => GetLayer(layerKey).Enter(item);

        public void EnterAll(object layerKey, IEnumerable<T> items)
            => GetLayer(layerKey).EnterAll(items);

        public void EnterAll(object layerKey)
            => GetLayer(layerKey).EnterAll();

        public void Enter(object layerKey, params T[] items)
            => GetLayer(layerKey).Enter(items);

        // Enter: default
        public void Enter(T item)
            => GetLayer(defaultLayerKey).Enter(item);

        public void EnterAll(IEnumerable<T> items)
            => GetLayer(defaultLayerKey).EnterAll(items);

        public void EnterAll()
            => GetLayer(defaultLayerKey).EnterAll();

        public void Enter(params T[] items)
            => GetLayer(defaultLayerKey).Enter(items);

        // Enter: generic
        public void Enter<TLayer>(T item)
            => GetLayer<TLayer>().Enter(item);

        public void EnterAll<TLayer>(IEnumerable<T> items)
            => GetLayer<TLayer>().EnterAll(items);

        public void EnterAll<TLayer>()
            => GetLayer<TLayer>().EnterAll();

        public void Enter<TLayer>(T[] items)
            => GetLayer<TLayer>().Enter(items);



        // Exit: layer
        public void Exit(object layerKey, T item)
            => GetLayer(layerKey).Exit(item);

        public void ExitAll(object layerKey, IEnumerable<T> items)
            => GetLayer(layerKey).ExitAll(items);

        public void ExitAll(object layerKey)
            => GetLayer(layerKey).ExitAll();

        public void Exit(object layerKey, params T[] items)
            => GetLayer(layerKey).Exit(items);

        // Exit: default
        public void Exit(T item)
            => GetLayer(defaultLayerKey).Exit(item);

        public void ExitAll(IEnumerable<T> items)
            => GetLayer(defaultLayerKey).ExitAll(items);

        public void ExitAll()
            => GetLayer(defaultLayerKey).ExitAll();

        public void Exit(params T[] items)
            => GetLayer(defaultLayerKey).Exit(items);

        // Exit: generic
        public void Exit<TLayer>(T item)
            => GetLayer<TLayer>().Exit(item);

        public void ExitAll<TLayer>(IEnumerable<T> items)
            => GetLayer<TLayer>().ExitAll(items);

        public void ExitAll<TLayer>()
            => GetLayer<TLayer>().ExitAll();

        public void Exit<TLayer>(T[] items)
            => GetLayer<TLayer>().Exit(items);



        public bool DidEnter<TLayer>(T item)
            => GetLayer<TLayer>().DidEnter(item);



        public void Toggle<TLayer>(bool enter, T item) =>
            GetLayer<TLayer>().Toggle(enter, item);

        public void ToggleAll<TLayer>(Func<T, bool> predicate)
        {
            var layer = GetLayer<TLayer>();

            foreach (T item in list)
                layer.Toggle(predicate(item), item);
        }



        // Next:
        public void Next()
            => GetLayer(defaultLayerKey).Next();

        public void Next(object layerKey)
            => GetLayer(layerKey).Next();

        public void Next<TLayer>()
            => GetLayer<TLayer>().Next();

        public void Next(SelectBoundMode boundMode)
            => GetLayer(defaultLayerKey).Next(boundMode);

        public void Next(object layerKey, SelectBoundMode boundMode)
            => GetLayer(layerKey).Next(boundMode);

        public void Next<TLayer>(SelectBoundMode boundMode)
            => GetLayer<TLayer>().Next(boundMode);



        // Previous:
        public void Previous()
            => GetLayer(defaultLayerKey).Previous();

        public void Previous(object layerKey)
            => GetLayer(layerKey).Previous();

        public void Previous<TLayer>()
            => GetLayer<TLayer>().Previous();
            
        public void Previous(SelectBoundMode boundMode)
            => GetLayer(defaultLayerKey).Previous(boundMode);

        public void Previous(object layerKey, SelectBoundMode boundMode)
            => GetLayer(layerKey).Previous(boundMode);

        public void Previous<TLayer>(SelectBoundMode boundMode)
            => GetLayer<TLayer>().Previous(boundMode);
    }
}
