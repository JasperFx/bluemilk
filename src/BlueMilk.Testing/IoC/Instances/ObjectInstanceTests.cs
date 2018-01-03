using BlueMilk.IoC.Instances;
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
    }
}