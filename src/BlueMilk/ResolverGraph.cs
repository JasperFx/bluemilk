using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BlueMilk.IoC;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;

namespace BlueMilk
{
    public class ResolverGraph
    {
        public static ResolverGraph Empty()
        {
            return Scope.Empty().Resolvers;
        }
        
        private readonly NewServiceGraph _services;
        public readonly IDictionary<Type, IResolver> ByType = new ConcurrentDictionary<Type, IResolver>();
        public readonly IDictionary<Type, IDictionary<string, IResolver>> ByTypeAndName = new ConcurrentDictionary<Type, IDictionary<string, IResolver>>();

        public ResolverGraph(NewServiceGraph services)
        {
            _services = services;
        }

        public void Register(Instance instance, IResolver resolver)
        {
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
        

    }
}