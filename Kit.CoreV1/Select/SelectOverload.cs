using System;
namespace Kit.CoreV1
{
    public partial class Select<T>
    {
        // Enter: layer
        public void Enter(object layerKey, T item)
            => GetLayer(layerKey).Enter(item);

        public void Enter(object layerKey, params T[] items)
            => GetLayer(layerKey).Enter(items);

        public void EnterAll(object layerKey, T item)
            => GetLayer(layerKey).EnterAll();

        // Enter: default
        public void Enter(T item)
            => GetLayer(defaultLayerKey).Enter(item);

        public void Enter(params T[] items)
            => GetLayer(defaultLayerKey).Enter(items);

        public void EnterAll()
            => GetLayer(defaultLayerKey).EnterAll();

        // Enter: generic
        public void Enter<TLayer>(T item)
            => GetLayer<TLayer>().Enter(item);

        public void Enter<TLayer>(T[] items)
            => GetLayer<TLayer>().Enter(items);

        public void EnterAll<TLayer>()
            => GetLayer<TLayer>().EnterAll();



        // Exit: layer
        public void Exit(object layerKey, T item)
            => GetLayer(layerKey).Exit(item);

        public void Exit(object layerKey, params T[] items)
            => GetLayer(layerKey).Exit(items);

        public void ExitAll(object layerKey, T item)
            => GetLayer(layerKey).ExitAll();

        // Exit: default
        public void Exit(T item)
            => GetLayer(defaultLayerKey).Exit(item);

        public void Exit(params T[] items)
            => GetLayer(defaultLayerKey).Exit(items);

        public void ExitAll()
            => GetLayer(defaultLayerKey).ExitAll();

        // Exit: generic
        public void Exit<TLayer>(T item)
            => GetLayer<TLayer>().Exit(item);

        public void Exit<TLayer>(T[] items)
            => GetLayer<TLayer>().Exit(items);

        public void ExitAll<TLayer>()
            => GetLayer<TLayer>().ExitAll();



        public bool DidEnter<TLayer>(T item)
            => GetLayer<TLayer>().DidEnter(item);



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
