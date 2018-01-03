using System.Linq;
using BlueMilk.IoC.Instances;
using BlueMilk.Testing.TargetTypes;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC
{
    public class ServiceFamilyTester
    {
        [Fact]
        public void the_last_instance_is_the_default()
        {
            var family = new ServiceFamily(typeof(IWidget), new Instance[]{
                ConstructorInstance.For<IWidget, AWidget>(),
                ConstructorInstance.For<IWidget, ColorWidget>(), 
            });
            
            family.Default.ImplementationType.ShouldBe(typeof(ColorWidget));
        }
        
        [Fact]
        public void make_all_the_names_unique()
        {
            var family = new ServiceFamily(typeof(IWidget), new Instance[]{
                ConstructorInstance.For<IWidget, AWidget>(),
                ConstructorInstance.For<IWidget, AWidget>(),
                ConstructorInstance.For<IWidget, AWidget>(),
                ConstructorInstance.For<IWidget, ColorWidget>(), 
                ConstructorInstance.For<IWidget, ColorWidget>(), 
                ConstructorInstance.For<IWidget, MoneyWidget>(), 
            });
            
            family.Instances.ContainsKey("AWidget1").ShouldBeTrue();
            family.Instances.ContainsKey("AWidget2").ShouldBeTrue();
            family.Instances.ContainsKey("AWidget3").ShouldBeTrue();
            family.Instances.ContainsKey("ColorWidget1").ShouldBeTrue();
            family.Instances.ContainsKey("ColorWidget2").ShouldBeTrue();
            family.Instances.ContainsKey("MoneyWidget").ShouldBeTrue();
        }
    }
}