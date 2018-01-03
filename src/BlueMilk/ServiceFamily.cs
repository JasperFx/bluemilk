﻿using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.IoC.Instances;

namespace BlueMilk
{
    public class ServiceFamily
    {
        private readonly Dictionary<string, Instance> _instances = new Dictionary<string, Instance>();
        
        
        public Type ServiceType { get; }

        public ServiceFamily(Type serviceType, Instance[] instances)
        {
            ServiceType = serviceType;

            Default = instances.Last();

            makeNamesUnique(instances);

            foreach (var instance in instances)
            {
                _instances.Add(instance.Name, instance);
            }
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

        public IReadOnlyDictionary<string, Instance> Instances => _instances;
    }
}