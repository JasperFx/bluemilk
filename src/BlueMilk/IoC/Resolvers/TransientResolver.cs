using System;

namespace BlueMilk.IoC.Resolvers
{
    public abstract class TransientResolver<T> : IResolver
    {
        public object Resolve(Scope scope)
        {
            var service = Build(scope);
            if (service is IDisposable)
            {
                scope.Disposables.Add((IDisposable) service);
            }

            return service;
        }
        
        public abstract object Build(Scope scope);

        public Type ServiceType => typeof(T);
    }

    public class NoArgTransientResolver<T> : TransientResolver<T> where T : new()
    {
        public override object Build(Scope scope)
        {
            return new T();
        }
    }
}