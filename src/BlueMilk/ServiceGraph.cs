using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.Compilation;
using BlueMilk.IoC;
using BlueMilk.IoC.Instances;
using BlueMilk.Util;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk
{
    public class ServiceGraph : IDisposable
    {
        private readonly Scope _rootScope;
        private readonly object _familyLock = new object();
        
        
        private readonly Dictionary<Type, ServiceFamily> _families = new Dictionary<Type, ServiceFamily>();
        private readonly IList<IFamilyPolicy> _familyPolicies = new List<IFamilyPolicy>();
        
        public ServiceGraph(IServiceCollection services, Scope rootScope)
        {
            _rootScope = rootScope;
            Services = services;
            Resolvers = new ResolverGraph();
        }

        public void Initialize()
        {
            // TODO -- will need to be able to use custom family policies

            organizeIntoFamilies(Services);

            
            


            // TODO -- any validations


            buildOutMissingResolvers();
        }

        private void buildOutMissingResolvers()
        {
            planResolutionStrategies();

            var requiresGenerated = generateDynamicAssembly();

            var noGeneration = instancesWithoutResolver().Where(x => !requiresGenerated.Contains(x));

            Resolvers.Register(_rootScope, noGeneration);
            Resolvers.Register(_rootScope, requiresGenerated);
        }

        private IEnumerable<Instance> instancesWithoutResolver()
        {
            return AllInstances().Where(x => !x.HasCreatedResolver);
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
        public ResolverGraph Resolvers { get; }

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
            return ctor.GetParameters().All(x => FindDefault(x.ParameterType) != null || Resolvers.ByType.ContainsKey(x.ParameterType));
        }

        public void Dispose()
        {
            foreach (var instance in AllInstances().OfType<IDisposable>())
            {
                instance.SafeDispose();
            }
        }

        private readonly Stack<Instance> _chain = new Stack<Instance>();
        public void StartingToPlan(Instance instance)
        {
            if (_chain.Contains(instance))
            {
                throw new InvalidOperationException("Bi-directional dependencies detected:" + Environment.NewLine + _chain.Select(x => x.ToString()).Join(Environment.NewLine));
            }
            
            _chain.Push(instance);
        }

        public void FinishedPlanning()
        {
            _chain.Pop();
        }

        public static ServiceGraph Empty()
        {
            return Scope.Empty().ServiceGraph;
        }
    }
}