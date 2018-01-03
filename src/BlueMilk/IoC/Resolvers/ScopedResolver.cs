using System;

namespace BlueMilk.IoC.Resolvers
{
    public abstract class ScopedResolver<T> : IResolver
    {
        public Type ServiceType => typeof(T);

        public object Resolve(Scope scope)
        {
            if (!scope.TryFind(out T service))
            {
                service = (T) Build(scope);
                
                // TODO -- will need to watch whether this is the default or not
                scope.Register(ServiceType, service);
                if (service is IDisposable) scope.RegisterDisposable(service as IDisposable);
            }

            return service;
        }

        public abstract object Build(Scope scope);
    }
}