using BlueMilk.IoC.Instances;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC.Instances
{
    public class LambdaInstanceTests
    {
        [Fact]
        public void derive_the_default_name()
        {
            LambdaInstance.For(s => new Clock())
                .Name.ShouldBe(nameof(Clock));
        }
    }
}