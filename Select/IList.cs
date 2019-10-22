using System.Collections;
using System.Collections.Generic;

namespace Kit
{
    public partial class Select<T>
    {
        public T this[int index] { get => list[index]; set => list[index] = value; }

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (list.Contains(item))
                throw new System.Exception($"oups, cannot add an item twice ({item})");

            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
        }

        public bool Remove(T item)
        {
            foreach (Layer layer in layers.Values)
                layer.Exit(item);

            return list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            T item = list[index];

            foreach (Layer layer in layers.Values)
                layer.Exit(item);

            list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
