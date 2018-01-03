using System;
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
    }

    public class ConstructorInstance : Instance
    {
        public ConstructorInstance(Type serviceType, Type implementationType, ServiceLifetime lifetime) : base(serviceType, implementationType, lifetime)
        {
        }
    }

    public class LambdaInstance : Instance
    {
        public LambdaInstance(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime) : base(serviceType, factory, lifetime)
        {
        }
    }

    public class ObjectInstance : Instance
    {
        public static ObjectInstance For<T>(T @object)
        {
            return new ObjectInstance(typeof(T), @object);
        }
        
        public ObjectInstance(Type serviceType, object instance) : base(serviceType, instance)
        {
        }
        
        
    }
}