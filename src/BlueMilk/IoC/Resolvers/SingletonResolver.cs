﻿using System;
using Baseline;

namespace BlueMilk.IoC.Resolvers
{
    public abstract class SingletonResolver<T> : IResolver where T : class
    {
        private readonly Scope _topLevelScope;
        private readonly object _locker = new object();
        
        public Type ServiceType => typeof(T);

        private T _service;
        
        public SingletonResolver(Scope topLevelScope)
        {
            _topLevelScope = topLevelScope;
        }

        public object Resolve(Scope scope)
        {
            if (_service != null) return _service;

            lock (_locker)
            {
                if (_service == null)
                {
                    _service = Build(_topLevelScope);
                    if (_service is IDisposable)
                    {
                        _topLevelScope.Disposables.Add((IDisposable) _service);
                    }
                    
                    // TODO -- will need to also do this by name.
                    // May not do this by default in some cases
                    _topLevelScope.Register(typeof(T), _service);
                }
            }

            return _service;
        }
        
        public abstract T Build(Scope scope);
        
        public string Name { get; set; }
        public int Hash { get; set; }
    }
}