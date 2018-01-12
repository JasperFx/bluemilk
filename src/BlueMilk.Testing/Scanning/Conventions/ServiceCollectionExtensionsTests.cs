﻿using System.Linq;
using BlueMilk.Scanning.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using StructureMap.Testing.Widget;
using Xunit;

namespace BlueMilk.Testing.Scanning.Conventions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void find_the_connected_concretions()
        {
            var services = new ServiceRegistry();
            services.ConnectedConcretions()
                .ShouldBeSameAs(services.ConnectedConcretions());
        }
        
        [Fact]
        public void add_type_on_an_empty_set()
        {
            var services = new ServiceRegistry();
            services.AddSingleton(this);

            services.AddType(typeof(IWidget), typeof(AWidget));

            var widgetDescriptor = services.Single(x => x.ServiceType == typeof(IWidget));
            widgetDescriptor.ServiceType.ShouldBe(typeof(IWidget));
            widgetDescriptor.ImplementationType.ShouldBe(typeof(AWidget));
        }

        [Fact]
        public void add_type_on_new_implementation_type()
        {
            var services = new ServiceRegistry();
            services.AddSingleton(this);

            services.AddType(typeof(IWidget), typeof(AWidget));
            services.AddType(typeof(IWidget), typeof(MoneyWidget));

            services.Where(x => x.ServiceType == typeof(IWidget))
                .Select(x => x.ImplementationType)
                .ShouldHaveTheSameElementsAs(typeof(AWidget), typeof(MoneyWidget));
        }

        [Fact]
        public void do_not_add_twice()
        {
            var services = new ServiceRegistry();
            services.AddSingleton(this);

            services.AddType(typeof(IWidget), typeof(AWidget));
            services.AddType(typeof(IWidget), typeof(AWidget));
            services.AddType(typeof(IWidget), typeof(MoneyWidget));
            services.AddType(typeof(IWidget), typeof(MoneyWidget));

            services.Where(x => x.ServiceType == typeof(IWidget))
                .Select(x => x.ImplementationType)
                .ShouldHaveTheSameElementsAs(typeof(AWidget), typeof(MoneyWidget));
        }
    }
}
