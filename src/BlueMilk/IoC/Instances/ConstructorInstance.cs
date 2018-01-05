using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public enum CreationStyle
    {
        InlineSingleton,
        Generated,
        NoArg
    }
    
    public class ConstructorInstance : Instance
    {
        public Type ImplementationType { get; }
        public static readonly string NoPublicConstructors = "No public constructors";
        public static readonly string NoPublicConstructorCanBeFilled = "Cannot fill the dependencies of any of the public constructors";
        
        public static ConstructorInstance For<T>(ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            return For<T, T>(lifetime);
        }
        
        public static ConstructorInstance For<T, TConcrete>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TConcrete : T
        {
            return new ConstructorInstance(typeof(T), typeof(TConcrete), lifetime);
        } 
        
        public ConstructorInstance(Type serviceType, Type implementationType, ServiceLifetime lifetime) : base(serviceType, lifetime)
        {
            ImplementationType = implementationType;
            Name = implementationType.NameInCode();
            
        }
        
        public CreationStyle CreationStyle { get; private set; }
        
        public ConstructorInfo Constructor { get; private set; }
        
        public Type ResolverBaseType { get; private set; }

        public override IResolver BuildResolver(ResolverGraph resolvers, Scope rootScope)
        {
            return null;
        }

        protected override IEnumerable<Instance> createPlan(NewServiceGraph services)
        {
            Constructor = DetermineConstructor(services, ImplementationType, out string message);

            if (message.IsNotEmpty()) ErrorMessages.Add(message);

            // TODO -- more here!
            return base.createPlan(services);
        }

        public static ConstructorInfo DetermineConstructor(NewServiceGraph services, Type implementationType, out string message)
        {
            message = null;
            
            var constructors = implementationType
                .GetConstructors();
            
           
            if (constructors.Any())
            {
                var ctor = constructors
                    .OrderByDescending(x => x.GetParameters().Length)
                    .FirstOrDefault(services.CouldBuild);

                if (ctor == null)
                {
                    message = NoPublicConstructorCanBeFilled;
                }

                return ctor;
            }

            message = NoPublicConstructors;
            
            return null;
        }
    }
}