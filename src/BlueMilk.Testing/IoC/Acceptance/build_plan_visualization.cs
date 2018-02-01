using System;
using Microsoft.Extensions.DependencyInjection;
using StructureMap.Testing.Widget;
using Xunit;

namespace BlueMilk.Testing.IoC.Acceptance
{
    public class build_plan_visualization
    {
        [Fact]
        public void make_it_clean()
        {
            var container = new Container(_ =>
            {
                _.AddSingleton<IWidget, AWidget>();
                _.AddTransient<WidgetUser>();
            });

            container.GetInstance<WidgetUser>();
            var plan = container.Model.For<WidgetUser>().Default.GetBuildPlan();
            
            plan.ShouldContain("public override BlueMilk.Testing.IoC.Acceptance.WidgetUser Build(BlueMilk.IoC.Scope scope)");
        }    
    }
}