using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using StructureMap.Testing.Widget;
using StructureMap.Testing.Widget3;
using Xunit;

namespace BlueMilk.Testing.IoC.Acceptance
{
    public class configure_container
    {
        [Fact]
        public void add_all_new_services()
        {
            var container = new Container(_ => { _.AddTransient<IWidget, RedWidget>(); });
            
            container.Configure(_ => _.AddTransient<IService, WhateverService>());

            container.GetInstance<IService>()
                .ShouldBeOfType<WhateverService>();
        }
        
        [Fact]
        public void add_to_existing_family()
        {
            var container = new Container(_ =>
            {
                _.AddTransient<IWidget, RedWidget>();
            });
            
            container.Configure(_ =>
            {
                _.AddTransient<IWidget, BlueWidget>();
                _.AddTransient<IWidget, GreenWidget>();
            });
            
            container.GetInstance<IWidget>()
                .ShouldBeOfType<GreenWidget>();
            
            container.GetAllInstances<IWidget>()
                .Select(x => x.GetType())
                .ShouldHaveTheSameElementsAs(typeof(RedWidget), typeof(BlueWidget), typeof(GreenWidget));

        }
    }
}