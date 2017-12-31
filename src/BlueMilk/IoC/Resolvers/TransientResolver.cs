using System;

namespace BlueMilk.IoC.Resolvers
{
    public abstract class TransientResolver<T> : IResolver
    {
        public abstract object Resolve(Scope scope);

        public Type ServiceType => typeof(T);
    }
}