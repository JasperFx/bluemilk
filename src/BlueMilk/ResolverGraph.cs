using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BlueMilk.IoC.Resolvers;

namespace BlueMilk
{
    public class ResolverGraph
    {
        private readonly ServiceGraph _services;
        public readonly IDictionary<Type, IResolver> ByType = new ConcurrentDictionary<Type, IResolver>();
        public readonly IDictionary<Type, IDictionary<string, IResolver>> ByTypeAndName = new ConcurrentDictionary<Type, IDictionary<string, IResolver>>();

        public ResolverGraph(ServiceGraph services)
        {
            _services = services;
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