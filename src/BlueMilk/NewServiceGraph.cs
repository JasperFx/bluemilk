using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Baseline;
using BlueMilk.IoC.Instances;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk
{
    public class NewServiceGraph
    {
        private readonly object _familyLock = new object();
        public readonly IServiceCollection Services;
        private readonly Dictionary<Type, ServiceFamily> _families = new Dictionary<Type, ServiceFamily>();
        private readonly IList<IFamilyPolicy> _familyPolicies = new List<IFamilyPolicy>();
        
        public NewServiceGraph(IServiceCollection services)
        {
            // TODO -- will need to be able to use custom family policies
            
            
            Services = services;

            services
                .Where(x => !x.ServiceType.IsGenericType)
                .Select(Instance.For)
                .GroupBy(x => x.ServiceType)
                .Select(x => new ServiceFamily(x.Key, x.ToArray()))
                .Each(family => _families.Add(family.ServiceType, family));
        }

        public IReadOnlyDictionary<Type, ServiceFamily> Families => _families;

        public ServiceFamily FindFamily(Type serviceType)
        {
            if (_families.ContainsKey(serviceType)) return _families[serviceType];

            lock (_familyLock)
            {
                if (_families.ContainsKey(serviceType)) return null;
                
                var family = _familyPolicies.FirstValue(x => x.Build(serviceType, this));
                _families.Add(serviceType, family); // Legal to be null here

                return family;
            }
        }
        
        public Instance FindDefault(Type serviceType)
        {
            return FindFamily(serviceType)?.Default;
        }

        public Instance[] FindAll(Type serviceType)
        {
            return FindFamily(serviceType)?.Instances.Values.ToArray() ?? new Instance[0];
        }
        
        public bool CouldBuild(ConstructorInfo ctor)
        {
            return ctor.GetParameters().All(x => FindDefault(x.ParameterType) != null);
        }

    }
}