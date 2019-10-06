using System.Collections.Generic;

namespace Kit.CoreV1
{
    public partial class Event
    {
        public partial class Listener
        {
            // 'IsContainer' is used by While<>() to allow invocation 
            // even if parent listener has 'ChildrenDisabled' = true
            public bool IsContainer { get; set; } = false;
            public bool ChildrenDisabled { get; set; } = false;

            List<Listener> children = new List<Listener>();

            Listener parent, root;

            public void AddChild(Listener child)
            {
                child.root = root ?? this;
                child.parent = this;
                children.Add(child);
            }

            public IEnumerable<Listener> GetAncestors()
            {
                Listener current = parent;

                while (current != null)
                {
                    yield return current;

                    current = current.parent;
                }
            }

            public IEnumerable<Listener> GetDescendants()
            {
                foreach(Listener child in children)
                {
                    yield return child;

                    foreach (Listener grandChild in child.GetDescendants())
                        yield return grandChild;
                }
            }

            public bool DisabledByAncestor()
            {
                foreach (var ancestor in GetAncestors())
                    if (ancestor.ChildrenDisabled)
                        return true;

                return false;
            }

            // 'IsEnabled' = true if the listener is a container (to allow While process)
            public bool IsEnabled => IsContainer || !DisabledByAncestor();

            public static Listener operator +(Listener lhs, Listener rhs)
            {
                lhs.AddChild(rhs);

                return lhs;
            }
        }
    }
}
