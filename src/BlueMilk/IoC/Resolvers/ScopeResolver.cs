using System;

namespace BlueMilk.IoC.Resolvers
{
    public class ScopeResolver<T> : IResolver
    {
        public object Resolve(Scope scope)
        {
            return scope;
        }

        public Type ServiceType => typeof(T);
    }
}