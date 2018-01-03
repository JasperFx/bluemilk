using System;
using System.Reflection;
using BlueMilk.Codegen;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public enum CreationStyle
    {
        InlineSingleton,
        Resolver
    }
    
    public class ConstructorInstance : Instance
    {
        public static ConstructorInstance For<T, TConcrete>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TConcrete : T
        {
            return new ConstructorInstance(typeof(T), typeof(TConcrete), lifetime);
        } 
        
        public ConstructorInstance(Type serviceType, Type implementationType, ServiceLifetime lifetime) : base(serviceType, implementationType, lifetime)
        {
            Name = implementationType.NameInCode();
        }
        
        public CreationStyle CreationStyle { get; private set; }
        
        public ConstructorInfo Constructor { get; private set; }
        
        public Type ResolverBaseType { get; private set; }

        public override void RegisterResolver(Assembly dynamicAssembly, ResolverGraph resolvers)
        {
            throw new NotImplementedException();
        }
    }
}