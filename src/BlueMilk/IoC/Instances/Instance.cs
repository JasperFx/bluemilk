using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;


namespace BlueMilk.IoC.Instances
{
    public abstract class Instance
    {
        internal IEnumerable<Assembly> ReferencedAssemblies()
        {
            yield return ServiceType.Assembly;
            yield return ImplementationType.Assembly;

            if (ServiceType.IsGenericType)
            {
                foreach (var type in ServiceType.GetGenericArguments())
                {
                    yield return type.Assembly;
                }
            }

            if (ImplementationType.IsGenericType)
            {
                foreach (var type in ImplementationType.GetGenericArguments())
                {
                    yield return type.Assembly;
                }
            }
        }

        public virtual bool IsLazy { get; } = false;
        
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
        public string Name { get; set; } = "default";
        
        public bool HasPlanned { get; protected internal set; }

        public void CreatePlan(ServiceGraph services)
        {
            if (HasPlanned) return;

            try
            {
                services.StartingToPlan(this);
                
            }
            catch (Exception e)
            {
                ErrorMessages.Add(e.Message);
                
                services.FinishedPlanning();
                HasPlanned = true;
                return;
            }
            
            var dependencies = createPlan(services) ?? Enumerable.Empty<Instance>();

            Dependencies = dependencies.Concat(dependencies.SelectMany(x => x.Dependencies)).Distinct().ToArray();

            services.ClearPlanning();
            HasPlanned = true;
        }


        public abstract Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot);

        protected virtual IEnumerable<Instance> createPlan(ServiceGraph services)
        {
            return Enumerable.Empty<Instance>();
        }

        public readonly IList<string> ErrorMessages = new List<string>();

        
        public Instance[] Dependencies { get; protected set; } = new Instance[0];


        public void Initialize(Scope rootScope)
        {
            if (Resolver != null) throw new InvalidOperationException("The Resolver has already been built for this Instance");
            
            Resolver = buildResolver(rootScope);

            if (Resolver == null)
            {
                if (ErrorMessages.Any())
                {
                    Resolver = new ErrorMessageResolver(this);
                }
                else
                {
                    throw new InvalidOperationException($"Instance {this} cannot build a valid resolver");
                }
            }

            Resolver.Hash = GetHashCode();
            Resolver.Name = Name;
        }

        public IResolver Resolver { get; protected set; }

        protected abstract IResolver buildResolver(Scope rootScope);
        

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

        public virtual Instance CloseType(Type serviceType, Type[] templateTypes)
        {
            return null;
        }
    }

    public class ErrorMessageResolver : IResolver
    {
        private readonly string _message;

        public ErrorMessageResolver(Instance instance)
        {
            ServiceType = instance.ServiceType;
            Name = instance.Name;
            Hash = instance.GetHashCode();

            _message = instance.ErrorMessages.Join(Environment.NewLine);
        }

        public object Resolve(Scope scope)
        {
            throw new BlueMilkException($"Cannot build registered instance {Name} of '{ServiceType.FullNameInCode()}':{Environment.NewLine}{_message}");
        }

        public Type ServiceType { get; }
        public string Name { get; set; }
        public int Hash { get; set; }
    }
}