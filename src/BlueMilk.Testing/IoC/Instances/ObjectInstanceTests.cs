using Baseline;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Planning;
using BlueMilk.IoC.Resolvers;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC.Instances
{
    public class ObjectInstanceTests
    {
        [Fact]
        public void derive_the_default_name()
        {
            ObjectInstance.For(new Clock())
                .Name.ShouldBe(nameof(Clock));
        }
        
        [Fact]
        public void build_a_resolver()
        {
            var clock = new Clock();
            var instance = ObjectInstance.For<IClock>(clock);

            instance.Initialize(null);
            instance.Resolver
                .ShouldBeSameAs(instance);
            
            instance.As<IResolver>().Resolve(null).ShouldBeSameAs(clock);
        }
        
        [Theory]
        [InlineData(BuildMode.Dependency)]
        [InlineData(BuildMode.Inline)]
        [InlineData(BuildMode.Build)]
        public void service_variable_is_injected_service_Variable(BuildMode mode)
        {
            var clock = new Clock();
            var instance = ObjectInstance.For<IClock>(clock);
            
            instance.CreateVariable(mode, null, false).ShouldBeOfType<InjectedServiceField>().Instance.ShouldBe(instance);
        }
    }
}