using BlueMilk.IoC.Instances;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC.Instances
{
    public class ConstructorInstanceTests
    {
        [Fact]
        public void derive_the_default_name()
        {
            ConstructorInstance.For<IClock, DisposableClock>()
                .Name.ShouldBe(nameof(DisposableClock));

        }
    }

}