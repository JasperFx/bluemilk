using System;

namespace BlueMilk.IoC.Resolvers
{
    public abstract class SingletonResolver<T> : IResolver where T : class
    {
        private readonly Scope _topLevelScope;
        private readonly object _locker = new object();
        
        public Type ServiceType => typeof(T);

        private T _service;
        
        protected SingletonResolver(Scope topLevelScope)
        {
            _topLevelScope = topLevelScope;
        }

        public object Resolve(Scope scope)
        {
            if (_service != null) return _service;
            
            lock (_locker)
            {
                if (_topLevelScope == null)
                {
                    _service = Build(_topLevelScope);
                }
            }

            return _service;
        }
        
        public abstract T Build(Scope scope);
    }
}