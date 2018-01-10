using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.Compilation;
using BlueMilk.IoC;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using BlueMilk.Util;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk
{
    public class ServiceGraph : IDisposable, IModel
    {
        private readonly Scope _rootScope;
        private readonly object _familyLock = new object();
        
        
        private readonly Dictionary<Type, ServiceFamily> _families = new Dictionary<Type, ServiceFamily>();
        public readonly IDictionary<Type, IResolver> ByType = new ConcurrentDictionary<Type, IResolver>();
        
        public ServiceGraph(IServiceCollection services, Scope rootScope)
        {
            _rootScope = rootScope;
            Services = services;

            FamilyPolicies = services
                .Where(x => x.ServiceType == typeof(IFamilyPolicy))
                .Select(x => x.ImplementationInstance.As<IFamilyPolicy>())
                .Concat(new IFamilyPolicy[]
                {
                    new CloseGenericFamilyPolicy(), 
                    new ConcreteFamilyPolicy(), 
                    new EmptyFamilyPolicy()
                })
                .ToArray();
            
            
            var policies = services.Where(x => x.ServiceType == typeof(IFamilyPolicy));
            services.RemoveAll(x => x.ServiceType == typeof(IFamilyPolicy));
            
            addScopeResolver<Scope>();
            addScopeResolver<IServiceProvider>();
            addScopeResolver<IContainer>();
            
            
        }

        public IFamilyPolicy[] FamilyPolicies { get; }

        private void addScopeResolver<T>()
        {
            // TODO -- will have to register by name as well?
            ByType[typeof(T)] = new ScopeResolver<T>();
        }

        public void Initialize()
        {
            // TODO -- will need to be able to use custom family policies

            organizeIntoFamilies(Services);

            
            


            // TODO -- any validations


            buildOutMissingResolvers();
        }
        
        
        
        public void RegisterResolver(Instance instance, IResolver resolver)
        {
            if (instance.IsDefault)
            {
                ByType[instance.ServiceType] = resolver;
            }
        }
        
        public void RegisterResolver(Scope rootScope, IEnumerable<Instance> instances)
        {
            foreach (var instance in instances.Where(x => x.Resolver == null).TopologicalSort(x => x.Dependencies, false))
            {
                instance.Initialize(rootScope);
                
                RegisterResolver(instance, instance.Resolver);
            }
        }
        
        private readonly object _locker = new object();

        
        public IResolver FindResolver(Type serviceType)
        {
            if (ByType.TryGetValue(serviceType, out var resolver))
            {
                return resolver;
            }

            lock (_locker)
            {
                if (_families.ContainsKey(serviceType)) return _families[serviceType].Default.Resolver;

                var family = TryToCreateMissingFamily(serviceType);

                return family.Default?.Resolver;
            }

        }
        
        public IResolver FindResolver(Type serviceType, string name)
        {
            if (_families.TryGetValue(serviceType, out var family))
            {
                return family.ResolverFor(name);
            }

            lock (_locker)
            {
                if (_families.ContainsKey(serviceType)) return _families[serviceType].ResolverFor(name);
                
                family = TryToCreateMissingFamily(serviceType);
                
                return family.ResolverFor(name);
            }
        }
        

        private void buildOutMissingResolvers()
        {
            planResolutionStrategies();

            var requiresGenerated = generateDynamicAssembly();

            var noGeneration = instancesWithoutResolver().Where(x => !requiresGenerated.Contains(x));

            RegisterResolver(_rootScope, noGeneration);
            RegisterResolver(_rootScope, requiresGenerated);
        }

        private IEnumerable<Instance> instancesWithoutResolver()
        {
            return AllInstances().Where(x => x.Resolver == null);
        }

        private Instance[] generateDynamicAssembly()
        {
            var generatedResolvers = instancesWithoutResolver()
                .OfType<IInstanceThatGeneratesResolver>()
                .Where(x => x.CreationStyle == CreationStyle.Generated)
                .ToArray();


            // TODO -- will need to get at the GenerationRules from somewhere
            var generatedAssembly = new GeneratedAssembly(new GenerationRules("Jasper.Generated"));
            AllInstances().Select(x => x.ImplementationType.Assembly)
                .Concat(AllInstances().Select(x => x.ServiceType.Assembly))
                .Distinct()
                .Each(a => generatedAssembly.Generation.Assemblies.Fill(a));

            foreach (var instance in generatedResolvers)
            {
                instance.GenerateResolver(generatedAssembly);
            }

            generatedAssembly.CompileAll();

            return generatedResolvers.OfType<Instance>().ToArray();
        }


        private void planResolutionStrategies()
        {
            while (AllInstances().Any(x => !x.HasPlanned))
            {
                foreach (var instance in AllInstances().Where(x => !x.HasPlanned).ToArray())
                {
                    instance.CreatePlan(this);
                }
            }
        }

        private void organizeIntoFamilies(IServiceCollection services)
        {
            services
                .Where(x => !x.ServiceType.IsGenericType && !x.ServiceType.CanBeCastTo<Instance>())
                .Select(Instance.For)
                .GroupBy(x => x.ServiceType)
                .Select(x => new ServiceFamily(x.Key, x.ToArray()))
                .Each(family => _families.Add(family.ServiceType, family));
        }

        public IServiceCollection Services { get; }

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
                
                var family = FamilyPolicies.FirstValue(x => x.Build(serviceType, this));
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
            return ctor.GetParameters().All(x => FindDefault(x.ParameterType) != null || ByType.ContainsKey(x.ParameterType));
        }

        public bool CouldBuild(Type concreteType)
        {
            var ctor = ConstructorInstance.DetermineConstructor(this, concreteType, out string message);
            return ctor != null && message.IsEmpty();
        }

        public void Dispose()
        {
            foreach (var instance in AllInstances().OfType<IDisposable>())
            {
                instance.SafeDispose();
            }
        }

        private readonly Stack<Instance> _chain = new Stack<Instance>();
        internal void StartingToPlan(Instance instance)
        {
            if (_chain.Contains(instance))
            {
                throw new InvalidOperationException("Bi-directional dependencies detected:" + Environment.NewLine + _chain.Select(x => x.ToString()).Join(Environment.NewLine));
            }
            
            _chain.Push(instance);
        }

        internal void FinishedPlanning()
        {
            _chain.Pop();
        }

        public static ServiceGraph Empty()
        {
            return Scope.Empty().ServiceGraph;
        }

        public static ServiceGraph For(Action<ServiceRegistry> configure)
        {
            var registry = new ServiceRegistry();
            configure(registry);
            
            return new Scope(registry).ServiceGraph;
        }

        public ServiceFamily TryToCreateMissingFamily(Type serviceType)
        {
            var family = FamilyPolicies.FirstValue(x => x.Build(serviceType, this));
            _families.Add(serviceType, family);
            
            buildOutMissingResolvers();

            return family;
        }

        IServiceFamilyConfiguration IModel.For<T>()
        {
            return FindFamily(typeof(T));
        }

        IServiceFamilyConfiguration IModel.For(Type type)
        {
            return FindFamily(type);
        }
    }
}