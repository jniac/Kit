using System;
using System.Linq;
using System.Collections.Generic;

namespace Kit.CoreV1.Tests
{
    public class Node : Tree<Node>
    {
        static List<Node> instances = new List<Node>();
        public static Node Get(int id) => instances[id];

        static int count;
        public readonly int id = count++;

        public Node()
        {
            instances.Add(this);
        }

        ////public Node parent;
        ////public Node first;
        ////public Node last;
        ////public Node next;

        ////public int GetChildCount()
        ////{
        ////    int childCount = 0;
        ////    Node node = first;

        ////    while (node)
        ////    {
        ////        childCount++;
        ////        node = node.next;
        ////    }

        ////    return childCount;
        ////}
        ////public int ChildCount { get => GetChildCount(); }

        ////public List<Node> GetChildren()
        ////{
        ////    var children = new List<Node>();
        ////    Node node = first;

        ////    while (node)
        ////    {
        ////        children.Add(node);
        ////        node = node.next;
        ////    }

        ////    return children;
        ////}
        ////public List<Node> Children { get => GetChildren(); }

        ////public int Level
        ////{
        ////    get
        ////    {
        ////        int level = 0;
        ////        Node node = parent;

        ////        while(node)
        ////        {
        ////            node = node.parent;
        ////            level++;
        ////        }

        ////        return level;
        ////    }
        ////}

        ////public void Walk(Action<Node> callback)
        ////{
        ////    callback(this);

        ////    Node node = first;

        ////    while(node)
        ////    {
        ////        node.Walk(callback);
        ////        node = node.next;
        ////    }
        ////}

        ////public Node Append(Node node)
        ////{
        ////    node.parent = this;

        ////    if (last)
        ////    {
        ////        last.next = node;
        ////        last = node;
        ////    }
        ////    else
        ////    {
        ////        first = last = node;
        ////    }

        ////    return this;
        ////}

        ////public Node Append(params Node[] nodes)
        ////{
        ////    foreach (var node in nodes)
        ////        Append(node);

        ////    return this;
        ////}

        //public override string ToString()
        //{
        //    return $"Node#{id}";
        //}

        public string ToLineString() => 
            string.Concat(Enumerable.Repeat(" . ", Level)
            .Append($"Node#{id} ({Level}|{ChildCount})"));

        public string ToGraphString()
        {
            var strings = new List<string>();

            Walk(node => strings.Add(node.ToLineString()));

            return string.Join("\n", strings);
        }

        public static implicit operator bool(Node node) => node != null;
    }
}
