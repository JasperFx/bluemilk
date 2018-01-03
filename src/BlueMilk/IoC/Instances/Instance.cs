using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlueMilk.Compilation;
using BlueMilk.IoC.Planning;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public abstract class Instance : ServiceDescriptor
    {
        public static Instance For(ServiceDescriptor service)
        {
            if (service is Instance instance) return instance;
            
            if (service.ImplementationInstance != null) return new ObjectInstance(service.ServiceType, service.ImplementationInstance);
            
            if (service.ImplementationFactory != null) return new LambdaInstance(service.ServiceType, service.ImplementationFactory, service.Lifetime);

            return new ConstructorInstance(service.ServiceType, service.ImplementationType, service.Lifetime);
        }
        
        protected Instance(Type serviceType, Type implementationType, ServiceLifetime lifetime) : base(serviceType, implementationType, lifetime)
        {
        }

        protected Instance(Type serviceType, object instance) : base(serviceType, instance)
        {
        }

        protected Instance(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime) : base(serviceType, factory, lifetime)
        {
        }

        public string Name { get; set; }
        
        public bool HasPlanned { get; protected internal set; }

        public void CreatePlan(NewServiceGraph services)
        {
            var dependencies = createPlan(services);

            Dependencies = dependencies.Concat(dependencies.SelectMany(x => x.Dependencies)).Distinct().ToArray();

            HasPlanned = true;
        }

        protected virtual IEnumerable<Instance> createPlan(NewServiceGraph services)
        {
            return Enumerable.Empty<Instance>();
        }

        public readonly IList<string> ErrorMessages = new List<string>();
        
        public BuildStep BuildStep { get; protected set; }

        public virtual void GenerateCode(ISourceWriter writer)
        {
            // Nothing
        }
        
        public Instance[] Dependencies { get; protected set; } = new Instance[0];


        public abstract void RegisterResolver(Assembly dynamicAssembly, ResolverGraph resolvers, Scope rootScope);

        public bool IsDefault { get; set; } = false;
    }
}