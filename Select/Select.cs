﻿#pragma warning disable RECS0108 // Signale les champs statiques dans les types génériques

using System;
using System.Linq;
using System.Collections.Generic;

namespace Kit
{
    public class SelectEvent : Event { }

    public class SelectEvent<T> : SelectEvent
    {
        public class Selected : SelectEvent<T> { }
        public class Change : SelectEvent<T> { }

        public T item;
        public Select<T> select;
        public Select<T>.Layer layer;
    }

    public partial class Select<T> : IList<T>
    {
        static int selectCount;
        public readonly int id = selectCount++;

        public object defaultLayerKey;

        List<T> list = new List<T>();

        public bool AutoRemoveItem { get; set; } = false;
        public bool AutoRemoveLayer { get; set; } = false;

        protected Dictionary<object, Layer> layers = new Dictionary<object, Layer>();

        public Select(params object[] layerKeys)
        {
            defaultLayerKey = $"Select#{id}";

            int index = 0;
            foreach(var key in layerKeys)
            {
                if (index == 0)
                    defaultLayerKey = key;

                layers.Add(key, new Layer(key, this));

                index++;
            }
        }

        public Select(Type e)
        {
            int index = 0;
            foreach (var key in Enum.GetValues(e))
            {
                if (index == 0)
                    defaultLayerKey = key;

                layers.Add(key, new Layer(key, this));

                index++;
            }
        }

        public Layer[] GetAllLayers()
        {
            return layers.Values.ToArray();
        }

        public Layer GetLayer(object layerKey)
        {
            if (!layers.TryGetValue(layerKey, out Layer layer))
            {
                layer = new Layer(layerKey, this);
                layers.Add(layerKey, layer);
            }

            return layer;
        }

        public Layer GetLayer<TLayer>()
            => GetLayer(typeof(TLayer));



        public string GetInfo()
        {
            string layerStr = "";

            if (layers.Count > 0)
            {
                int padLength = layers.Values.Max(v => v.GetName().Length);

                var strings = layers.Values.Select(layer
                    => $"  {layer.GetName().PadRight(padLength)} ({layer.Count}): {string.Join(",", layer.set)}");

                layerStr = "\n" + string.Join("\n", strings);
            }

            return $"Select ({list.Count}:{layers.Count}):{layerStr}";
        }

        public string GetDetailedInfo()
        {
            return $"{GetType().Name.Split('`')[0]}<{typeof(T).Name}>({list.Count}):\n"
                + string.Join("\n", list.Select((v, i) => $"#{i}: {v}"))
                + $"\n\nLayers({layers.Count}):\n\n"
                + string.Join("\n\n", layers.Values.Select(layer => layer.GetDetailedInfo()));
        }
    }
}

#pragma warning restore RECS0108 // Signale les champs statiques dans les types génériques
