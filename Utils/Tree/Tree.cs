#pragma warning disable RECS0108 // Signale les champs statiques dans les types génériques

using System;
using System.Collections.Generic;

namespace Kit
{
    // Tree.
    // Self-referecing generic class... wah.
    // Should be extended as "class Foo : Tree<Foo>"
    public class Tree<T> where T : Tree<T>
    {
        static int treeNodeCount;
        public readonly int nodeId = treeNodeCount++;

        public static implicit operator bool(Tree<T> node) => node != null;

        public T Parent { get; private set; } = null;
        public T First { get; private set; } = null;
        public T Last { get; private set; } = null;
        public T Previous { get; private set; } = null;
        public T Next { get; private set; } = null;

        public T Root 
        {
            get
            {
                T node = this as T;

                while (node.Parent)
                    node = node.Parent;

                return node;
            }
        }

        public bool IsRoot { get => Root == this; }
        public bool IsTail { get => First == null; }

        public T Append(T node)
        {
            node.Parent = this as T;

            if (Last)
            {
                node.Previous = Last;
                Last.Next = node;
                Last = node;
            }
            else
            {
                First = Last = node;
            }

            return this as T;
        }

        public T Append(params T[] nodes)
        {
            foreach (var node in nodes)
                Append(node);

            return this as T;
        }

        public bool ContainsChild(T node)
            => node.Parent == this;

        public bool Remove(T node)
        {
            if (ContainsChild(node))
            {
                if (node == First)
                    First = First.Next;

                if (node == Last)
                    Last = Last.Previous;

                if (node.Previous)
                   node.Previous.Next = node.Next;

                if (node.Next)
                    node.Next.Previous = node.Previous;

                node.Next = node.Previous = node.Parent = null;

                return true;
            }

            return false;
        }

        public T Walk(Action<T> callback, bool includeSelf = true)
        {
            if (includeSelf)
                callback(this as T);

            T node = First;

            while (node)
            {
                node.Walk(callback);
                node = node.Next;
            }

            return this as T;
        }

        public int ChildCount 
        {
            get
            {
                int childCount = 0;
                T node = First;

                while (node)
                {
                    childCount++;
                    node = node.Next;
                }

                return childCount;
            }
        }

        public List<T> Children 
        { 
            get 
            {
                var list = new List<T>();
                T node = First;

                while (node)
                {
                    list.Add(node);
                    node = node.Next;
                }

                return list;
            }
        }

        public int Level
        {
            get 
            {
                int level = 0;
                T node = Parent;

                while (node)
                {
                    node = node.Parent;
                    level++;
                }

                return level;
            }
        }

        public override string ToString()
            => $"{typeof(Tree<T>).Name}#{nodeId}(l:{Level},c:{ChildCount})";
    }
}

#pragma warning restore RECS0108 // Signale les champs statiques dans les types génériques
