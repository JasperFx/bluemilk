using Baseline;
using BlueMilk.IoC.Instances;
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

            instance.BuildResolver(null, null, null)
                .ShouldBeSameAs(instance);
            
            instance.As<IResolver>().Resolve(null).ShouldBeSameAs(clock);
        }
    }
}