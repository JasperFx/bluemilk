using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.Compilation;
using BlueMilk.IoC;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using BlueMilk.Scanning.Conventions;
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
            Services = services;



            // This should blow up pretty fast if it's no good
            applyScanners(services).Wait();
            
            _rootScope = rootScope;
            

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
            
            
            services.RemoveAll(x => x.ServiceType == typeof(IFamilyPolicy));
            
            addScopeResolver<Scope>();
            addScopeResolver<IServiceProvider>();
            addScopeResolver<IContainer>();
            
            
        }

        private async Task applyScanners(IServiceCollection services)
        {
            var scanners = services.Select(x => x.ImplementationInstance).OfType<AssemblyScanner>().ToArray();
            services.RemoveAll(x => x.ServiceType == typeof(AssemblyScanner));

            foreach (var scanner in scanners)
            {
                await scanner.ApplyRegistrations(services);
            }
                        
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
            return AllInstances().Where(x => x.Resolver == null && !x.ServiceType.IsOpenGeneric());
        }

        private Instance[] generateDynamicAssembly()
        {
            var generatedResolvers = instancesWithoutResolver()
                .OfType<IInstanceThatGeneratesResolver>()
                .Where(x => x.CreationStyle == CreationStyle.Generated)
                .ToArray();


            // TODO -- will need to get at the GenerationRules from somewhere
            var generatedAssembly = new GeneratedAssembly(new GenerationRules("Jasper.Generated"));
            AllInstances().Where(x => !x.ServiceType.IsOpenGeneric()).Select(x => x.ImplementationType.Assembly)
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
            while (AllInstances().Where(x => !x.ServiceType.IsOpenGeneric()).Any(x => !x.HasPlanned))
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
                .Where(x => x.ServiceType.Assembly != GetType().Assembly)
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

        public bool HasFamily(Type serviceType)
        {
            return _families.ContainsKey(serviceType);
        }
        
        public ServiceFamily ResolveFamily(Type serviceType)
        {
            if (_families.ContainsKey(serviceType)) return _families[serviceType];

            lock (_familyLock)
            {
                if (_families.ContainsKey(serviceType)) return _families[serviceType];

                return TryToCreateMissingFamily(serviceType);
            }
        }
        
        public Instance FindDefault(Type serviceType)
        {
            if (serviceType.IsSimple()) return null;
            
            return ResolveFamily(serviceType)?.Default;
        }

        public Instance[] FindAll(Type serviceType)
        {
            return ResolveFamily(serviceType)?.Instances.Values.ToArray() ?? new Instance[0];
        }
        
        public bool CouldBuild(ConstructorInfo ctor)
        {
            return ctor.GetParameters().All(x => ByType.ContainsKey(x.ParameterType) || FindDefault(x.ParameterType) != null);
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
            _families.SmartAdd(serviceType, family);
            
            buildOutMissingResolvers();

            return family;
        }

        IServiceFamilyConfiguration IModel.For<T>()
        {
            return ResolveFamily(typeof(T));
        }

        IServiceFamilyConfiguration IModel.For(Type type)
        {
            return ResolveFamily(type);
        }

        internal void ClearPlanning()
        {
            _chain.Clear();
        }
    }
}