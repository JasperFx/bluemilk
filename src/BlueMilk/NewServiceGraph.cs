using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Baseline;
using BlueMilk.IoC;
using BlueMilk.IoC.Instances;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk
{
    public class NewServiceGraph : IDisposable
    {
        private readonly Scope _rootScope;
        private readonly object _familyLock = new object();
        
        
        private readonly Dictionary<Type, ServiceFamily> _families = new Dictionary<Type, ServiceFamily>();
        private readonly IList<IFamilyPolicy> _familyPolicies = new List<IFamilyPolicy>();
        
        public NewServiceGraph(IServiceCollection services, Scope rootScope)
        {
            _rootScope = rootScope;
            // TODO -- will need to be able to use custom family policies
            
            
            Services = services;
            Resolvers = new ResolverGraph(this);
            

            // 1. Organize closed type services into ServiceFamily objects
            services
                .Where(x => !x.ServiceType.IsGenericType && !x.ServiceType.CanBeCastTo<Instance>())
                .Select(Instance.For)
                .GroupBy(x => x.ServiceType)
                .Select(x => new ServiceFamily(x.Key, x.ToArray()))
                .Each(family => _families.Add(family.ServiceType, family));
            
            // 2. Do planning on each instance
            foreach (var instance in AllInstances())
            {
                if (@instance.HasPlanned)
                {
                    instance.CreatePlan(this);
                }
            }
            
            // TODO -- any validations
            // TODO -- generate the dynamic assembly

            foreach (var instance in AllInstances())
            {
                var resolver = instance.BuildResolver(null, Resolvers, _rootScope);
                Resolvers.Register(instance, resolver);
            }
        }
        
        public IServiceCollection Services { get; }
        public ResolverGraph Resolvers { get; }

        public void CreatePlans()
        {


            // 3. TODO -- validate there are no errors? Maybe make that optional?


            // 4. Generate the code for the singleton initializer and generated resolvers
            var inlineSingletons = AllInstances()
                .OfType<ConstructorInstance>()
                .Where(x => x.CreationStyle == CreationStyle.InlineSingleton)
                .ToArray();

            var generatedResolvers = AllInstances()
                .OfType<ConstructorInstance>()
                .Where(x => x.CreationStyle == CreationStyle.Generated)
                .ToArray();

            var assembly = generateDynamicResolverAssembly(inlineSingletons, generatedResolvers);
        }

        private Assembly generateDynamicResolverAssembly(ConstructorInstance[] inlineSingletons, ConstructorInstance[] generatedResolvers)
        {
            return null;
        }

        public IEnumerable<Instance> AllInstances()
        {
            return _families.Values.SelectMany(x => x.All);
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

        public void Dispose()
        {
            foreach (var instance in AllInstances().OfType<IDisposable>())
            {
                instance.SafeDispose();
            }
        }
    }
}