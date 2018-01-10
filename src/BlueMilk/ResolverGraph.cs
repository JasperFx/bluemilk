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
        public static ResolverGraph Empty()
        {
            return Scope.Empty().Resolvers;
        }
        
        public readonly IDictionary<Type, IResolver> ByType = new ConcurrentDictionary<Type, IResolver>();
        public readonly IDictionary<Type, IDictionary<string, IResolver>> ByTypeAndName = new ConcurrentDictionary<Type, IDictionary<string, IResolver>>();

        public ResolverGraph()
        {
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

        public IResolver For(Type serviceType)
        {
            // TODO -- what if it's not already there?

            return ByType[serviceType];
        }

        public IResolver For(Type serviceType, string name)
        {
            // TODO -- what if it's not already there?
            return ByTypeAndName[serviceType][name];
        }

        public void Register(Scope rootScope, IEnumerable<Instance> instances)
        {
            foreach (var instance in instances.TopologicalSort(x => x.Dependencies, false))
            {
                var resolver = instance.BuildResolver(this, rootScope);
                Register(instance, resolver);
            }
        }
        

    }
}