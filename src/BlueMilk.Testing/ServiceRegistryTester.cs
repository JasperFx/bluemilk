﻿using System.Linq;
using BlueMilk.IoC.Instances;
using BlueMilk.Testing.TargetTypes;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing
{
    public class ServiceRegistryTester
    {
        [Fact]
        public void for_use()
        {
            var registry = new ServiceRegistry();
            registry.For<IWidget>().Use<AWidget>();

            var descriptor = registry.Single();
            
            var instance = descriptor.ImplementationInstance.ShouldBeOfType<ConstructorInstance>();
            instance.ImplementationType.ShouldBe(typeof(AWidget));

            descriptor.ServiceType.ShouldBe(typeof(IWidget));

        }

        [Fact]
        public void forsingleton_use()
        {
            var registry = new ServiceRegistry();
            registry.ForSingletonOf<IWidget>().Use<AWidget>();

            var descriptor = registry.Single();

            var instance = descriptor.ImplementationInstance.ShouldBeOfType<ConstructorInstance>();
            instance.ImplementationType.ShouldBe(typeof(AWidget));
            instance.Lifetime.ShouldBe(ServiceLifetime.Singleton);
                
            
            descriptor.ServiceType.ShouldBe(typeof(IWidget));
        }
    }
}
