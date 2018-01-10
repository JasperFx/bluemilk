﻿using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;

namespace BlueMilk
{
    public class ServiceFamily : IServiceFamilyConfiguration
    {
        private readonly Dictionary<string, Instance> _instances = new Dictionary<string, Instance>();


        public Type ServiceType { get; }

        public ServiceFamily(Type serviceType, params Instance[] instances)
        {
            foreach (var instance in instances)
            {
                instance.IsDefault = false;
            }

            if (instances.Any())
            {
                instances.Last().IsDefault = true;
            }

            ServiceType = serviceType;

            Default = instances.LastOrDefault();


            makeNamesUnique(instances);

            foreach (var instance in instances)
            {
                _instances.Add(instance.Name, instance);
            }

            All = instances;
        }

        // Has to be in order here
        public Instance[] All { get; }

        public IResolver ResolverFor(string name)
        {
            return Instances.ContainsKey(name) ? Instances[name].Resolver : null;
        }

        private void makeNamesUnique(IEnumerable<Instance> instances)
        {
            instances
                .GroupBy(x => x.Name)
                .Select(x => x.ToArray())
                .Where(x => x.Length > 1)
                .Each(array =>
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i].Name += (i + 1).ToString();
                    }
                });
        }

        public Instance Default { get; }

        IEnumerable<Instance> IServiceFamilyConfiguration.Instances => _instances.Values;

        bool IServiceFamilyConfiguration.HasImplementations()
        {
            return _instances.Any();
        }

        public IReadOnlyDictionary<string, Instance> Instances => _instances;
    }
}