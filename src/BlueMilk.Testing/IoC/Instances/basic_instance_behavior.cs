using BlueMilk.IoC.Instances;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC.Instances
{
    public class basic_instance_behavior
    {
        [Fact]
        public void for_returns_the_instance()
        {
            var @object = new ObjectInstance(typeof(string),"foo");
            Instance.For(@object)
                .ShouldBeSameAs(@object);
        }

        [Fact]
        public void for_object()
        {
            var descriptor = ServiceDescriptor.Singleton(typeof(string), "foo");
            var instance = Instance.For(descriptor)
                .ShouldBeOfType<ObjectInstance>();
            
            instance.ImplementationInstance.ShouldBe(descriptor.ImplementationInstance);
            instance.ServiceType.ShouldBe(descriptor.ServiceType);
            instance.Lifetime.ShouldBe(descriptor.Lifetime);
        }
        
        [Fact]
        public void for_concrete_type()
        {
            var descriptor = ServiceDescriptor.Scoped<IClock, DisposableClock>();
            var instance = Instance.For(descriptor)
                .ShouldBeOfType<ConstructorInstance>();
            
            instance.ImplementationType.ShouldBe(descriptor.ImplementationType);
            instance.ServiceType.ShouldBe(descriptor.ServiceType);
            instance.Lifetime.ShouldBe(descriptor.Lifetime);
        }
        
        [Fact]
        public void for_lambda()
        {
            var descriptor = ServiceDescriptor.Singleton<IClock>(s => new Clock());
            
            var instance = Instance.For(descriptor)
                .ShouldBeOfType<LambdaInstance>();
            
            instance.ImplementationFactory.ShouldBe(descriptor.ImplementationFactory);
            instance.ServiceType.ShouldBe(descriptor.ServiceType);
            instance.Lifetime.ShouldBe(descriptor.Lifetime);
        }
        
        
    }
}