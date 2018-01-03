using System;
using BlueMilk.Codegen;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
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
    }
}