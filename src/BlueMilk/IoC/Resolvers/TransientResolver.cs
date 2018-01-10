﻿using System;

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
        
        public string Name { get; set; }
        public int Hash { get; set; }
    }
}