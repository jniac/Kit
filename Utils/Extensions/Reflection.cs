using System;
using System.Linq;
using System.Reflection;

namespace Kit
{
    public static class ReflectionExtensions
    {
        public static bool IsStatic(this PropertyInfo source, bool nonPublic = false)
            => source.GetAccessors(nonPublic).Any(x => x.IsStatic);
    }

}
