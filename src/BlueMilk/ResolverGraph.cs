using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BlueMilk.IoC;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using BlueMilk.Util;

namespace BlueMilk
{
    public class ResolverGraph
    {
        private readonly ServiceGraph _services;

        public static ResolverGraph Empty()
        {
            return Scope.Empty().Resolvers;
        }
        
        public readonly IDictionary<Type, IResolver> ByType = new ConcurrentDictionary<Type, IResolver>();
        public readonly IDictionary<Type, IDictionary<string, IResolver>> ByTypeAndName = new ConcurrentDictionary<Type, IDictionary<string, IResolver>>();

        public ResolverGraph(ServiceGraph services)
        {
            _services = services;
            addScopeResolver<Scope>();
            addScopeResolver<IServiceProvider>();
            addScopeResolver<IContainer>();
        }

        private void addScopeResolver<T>()
        {
            ByType[typeof(T)] = new ScopeResolver<T>();
        }

        public void Register(Instance instance, IResolver resolver)
        {
            resolver.Hash = instance.GetHashCode();
            resolver.Name = instance.Name;

            instance.HasCreatedResolver = true;
            
            if (!ByTypeAndName.ContainsKey(instance.ServiceType))
            {
                ByTypeAndName[instance.ServiceType] = new ConcurrentDictionary<string, IResolver>();
            }
            
            ByTypeAndName[instance.ServiceType][instance.Name] = resolver;
            if (instance.IsDefault)
            {
                ByType[instance.ServiceType] = resolver;
            }
        }

        public void Register(Scope rootScope, IEnumerable<Instance> instances)
        {
            foreach (var instance in instances.TopologicalSort(x => x.Dependencies, false))
            {
                var resolver = instance.BuildResolver(this, rootScope);
                Register(instance, resolver);
            }
        }

        private readonly object _locker = new object();

        
        public IResolver Find(Type serviceType)
        {
            if (ByType.TryGetValue(serviceType, out var resolver))
            {
                return resolver;
            }

            lock (_locker)
            {
                if (!ByType.ContainsKey(serviceType))
                {
                    if (!_services.TryToFindMissingFamily(serviceType))
                    {
                        // Memoize the miss
                        ByType[serviceType] = null;
                    }
                }
            }

            return ByType[serviceType];

        }

        public IResolver Find(Type serviceType, string name)
        {
            if (ByTypeAndName.TryGetValue(serviceType, out var dictionary))
            {
                if (dictionary.TryGetValue(name, out var resolver))
                {
                    return resolver;
                }
                else
                {
                    // Memoize the misses
                    dictionary[name] = null;
                    return null;
                }
            }

            lock (_locker)
            {
                if (!ByTypeAndName.ContainsKey(serviceType))
                {
                    if (!_services.TryToFindMissingFamily(serviceType))
                    {
                        // Memoize the miss
                        dictionary = new ConcurrentDictionary<string, IResolver> {[name] = null};
                        ByTypeAndName[serviceType] = dictionary;
                        
                        return null;
                    }
                }
            }

            dictionary = ByTypeAndName[serviceType];
            if (dictionary.TryGetValue(name, out var found))
            {
                return found;
            }

            dictionary[name] = null; // memoize the miss
            return null;
        }
    }
}