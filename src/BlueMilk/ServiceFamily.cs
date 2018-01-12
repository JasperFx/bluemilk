﻿using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

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

        public override string ToString()
        {
            return $"{nameof(ServiceType)}: {ServiceType.FullNameInCode()}";
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

        public void AddType(Type concreteType)
        {
            if (!concreteType.IsConcrete())
            {
                throw new ArgumentOutOfRangeException(nameof(concreteType), $"{concreteType.FullNameInCode()} is not a concrete type");
            }
            
            if (!concreteType.CanBeCastTo(ServiceType)) return;

            if (_instances.Values.Any(x => x.ImplementationType == concreteType)) return;
            
            var instance = new ConstructorInstance(ServiceType, concreteType, ServiceLifetime.Transient);
            if (_instances.ContainsKey(instance.Name))
            {
                instance.Name += "_2";
            }
            
            _instances.Add(instance.Name, instance);
        }

        /// <summary>
        /// If the ServiceType is an open generic type, this method will create a 
        /// closed type copy of this PluginFamily
        /// </summary>
        /// <param name="types"></param>
        /// <param name="templateTypes"></param>
        /// <returns></returns>
        public ServiceFamily CreateTemplatedClone(Type serviceType, Type[] templateTypes)
        {
            if (!ServiceType.IsGenericType) throw new InvalidOperationException($"{ServiceType.FullNameInCode()} is not an open generic type");
            
            var instances = _instances.Values.Select(x => {
                var clone = x.CloseType(serviceType, templateTypes);
                if (clone == null) return null;

                clone.Name = x.Name;
                return clone;
            }).Where(x => x != null).ToArray();

            return new ServiceFamily(serviceType, instances);
        }
        
        
    }
}