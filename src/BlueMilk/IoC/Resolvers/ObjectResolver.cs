using System;

namespace BlueMilk.IoC.Resolvers
{
    public class ObjectResolver : IResolver
    {
        private readonly object _service;

        public ObjectResolver(Type serviceType, object service)
        {
            ServiceType = serviceType;
            _service = service;
        }

        public Type ServiceType { get; }

        public object Resolve(Scope scope)
        {
            return _service;
        }
    }
}