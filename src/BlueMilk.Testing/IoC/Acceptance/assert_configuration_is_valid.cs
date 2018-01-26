using BlueMilk.IoC;
using Microsoft.Extensions.DependencyInjection;
using StructureMap.Testing.Widget;
using Xunit;

namespace BlueMilk.Testing.IoC.Acceptance
{
    public class assert_configuration_is_valid
    {
        [Fact]
        public void happy_path()
        {
            var container = new Container(_ =>
            {
                _.AddTransient<IWidget, AWidget>();
                _.AddTransient<WidgetUser>();
                _.AddTransient<IWidget, BlueWidget>();
                _.AddTransient<WidgetIListUser>();
            });
            
            container.AssertConfigurationIsValid();
        }
        
        [Fact]
        public void unhappy_path_with_missing_dependency()
        {
            var container = new Container(_ =>
            {
                _.AddTransient<WidgetUser>();
            });
            
            Exception<ContainerValidationException>.ShouldBeThrownBy(() =>
            {
                container.AssertConfigurationIsValid();
            }).Message.ShouldContain("IWidget is not registered");
            
        }
        
        
    }
}