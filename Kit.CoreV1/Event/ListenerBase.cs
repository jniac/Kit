using System.Collections.Generic;

namespace Kit.CoreV1
{
    public partial class Event
    {
        public partial class Listener
        {
            public bool ChildrenDisabled { get; set; } = false;

            List<Listener> children = new List<Listener>();

            Listener parent, root;

            public void AddChild(Listener child)
            {
                child.root = root ?? this;
                child.parent = this;
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

            public bool GetIsEnabled()
            {
                foreach (var ancestor in GetAncestors())
                    if (ancestor.ChildrenDisabled)
                        return false;

                return true;
            }

            public bool IsEnabled { get => GetIsEnabled(); }

            public static Listener operator +(Listener lhs, Listener rhs)
            {
                lhs.AddChild(rhs);

                return lhs;
            }
        }
    }
}
