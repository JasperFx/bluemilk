using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlueMilk.Codegen;
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

        public override void RegisterResolver(Assembly dynamicAssembly, ResolverGraph resolvers, Scope rootScope)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Instance> createPlan(NewServiceGraph services)
        {
            var constructors = ImplementationType
                .GetConstructors();

            if (constructors.Any())
            {
                Constructor = constructors
                    .OrderByDescending(x => x.GetParameters().Length)
                    .FirstOrDefault(services.CouldBuild);

                if (Constructor == null)
                {
                    ErrorMessages.Add(NoPublicConstructorCanBeFilled);
                }
            }
            else
            {
                ErrorMessages.Add(NoPublicConstructors);
            }
            

            
            // TODO -- more here!
            return base.createPlan(services);

        }
    }
}