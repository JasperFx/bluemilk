using System.Linq;
using Baseline;
using BlueMilk.IoC.Instances;
using Shouldly;
using StructureMap.Testing.Widget;
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
            
            family.Default.As<ConstructorInstance>().ImplementationType.ShouldBe(typeof(ColorWidget));
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
        
        [Fact]
        public void stores_all_in_order()
        {
            var allInstances = new Instance[]{
                ConstructorInstance.For<IWidget, AWidget>(),
                ConstructorInstance.For<IWidget, AWidget>(),
                ConstructorInstance.For<IWidget, ColorWidget>(), 
                ConstructorInstance.For<IWidget, MoneyWidget>(), 
            };
            var family = new ServiceFamily(typeof(IWidget), allInstances);
            
            family.All.ShouldBe(allInstances);
        }
        
        [Fact]
        public void setting_the_is_default_property_on_instance()
        {
            var family = new ServiceFamily(typeof(IWidget), new Instance[]{
                ConstructorInstance.For<IWidget, AWidget>(),
                ConstructorInstance.For<IWidget, AWidget>(),
                ConstructorInstance.For<IWidget, AWidget>(),
                ConstructorInstance.For<IWidget, ColorWidget>(), 
                ConstructorInstance.For<IWidget, ColorWidget>(), 
                ConstructorInstance.For<IWidget, MoneyWidget>(), 
            });
            
            family.Instances["MoneyWidget"].IsDefault.ShouldBeTrue();
            family.Instances.Values.Where(x => x.Name != "MoneyWidget").Each(x => x.IsDefault.ShouldBeFalse());
        }
    }
}