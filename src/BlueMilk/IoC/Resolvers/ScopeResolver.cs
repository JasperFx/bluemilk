using System;
using BlueMilk.IoC.Instances;

namespace BlueMilk.IoC.Resolvers
{
    public class ScopeResolver<T> : IResolver
    {
        public object Resolve(Scope scope)
        {
            return scope;
        }

        public Type ServiceType => typeof(T);

        public string Name { get; set; } = "default";
        public int Hash { get; set; } = Instance.HashCode(typeof(T), "default");
    }
}