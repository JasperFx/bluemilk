using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Planning;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;


namespace BlueMilk.IoC.Instances
{
    public abstract class Instance
    {
        public Type ServiceType { get; }
        public Type ImplementationType { get; }

        public static Instance For(ServiceDescriptor service)
        {
            if (service.ImplementationInstance is Instance instance) return instance;
            
            if (service.ImplementationInstance != null) return new ObjectInstance(service.ServiceType, service.ImplementationInstance);
            
            if (service.ImplementationFactory != null) return new LambdaInstance(service.ServiceType, service.ImplementationFactory, service.Lifetime);

            return new ConstructorInstance(service.ServiceType, service.ImplementationType, service.Lifetime);
        }

        protected Instance(Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
            ImplementationType = implementationType;
        }

        public virtual bool RequiresServiceProvider => Dependencies.Any(x => x.RequiresServiceProvider);

        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;
        public string Name { get; set; }
        
        public bool HasPlanned { get; protected internal set; }

        public void CreatePlan(ServiceGraph services)
        {
            if (HasPlanned) return;

            services.StartingToPlan(this);
            
            var dependencies = createPlan(services) ?? Enumerable.Empty<Instance>();

            Dependencies = dependencies.Concat(dependencies.SelectMany(x => x.Dependencies)).Distinct().ToArray();

            services.FinishedPlanning();
            HasPlanned = true;
        }


        public abstract Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot);

        protected virtual IEnumerable<Instance> createPlan(ServiceGraph services)
        {
            return Enumerable.Empty<Instance>();
        }

        public readonly IList<string> ErrorMessages = new List<string>();

        
        public Instance[] Dependencies { get; protected set; } = new Instance[0];

        public abstract IResolver BuildResolver(ResolverGraph resolvers, Scope rootScope);

        public bool IsDefault { get; set; } = false;


        public sealed override int GetHashCode()
        {
            unchecked
            {
                return HashCode(ServiceType, Name);
            }
        }

        public static int HashCode(Type serviceType, string name = null)
        {
            return (serviceType.GetHashCode() * 397) ^ (name ?? "default").GetHashCode();
        }
    }
}