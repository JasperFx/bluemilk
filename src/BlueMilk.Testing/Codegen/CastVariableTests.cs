using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.Codegen
{
    public class CastVariableTests
    {
        [Fact]
        public void does_the_cast()
        {
            var inner = Variable.For<Basketball>();
            var cast = new CastVariable(inner, typeof(Ball));
            
            cast.Usage.ShouldBe($"(({typeof(Ball).FullNameInCode()}){inner.Usage})");
        }
    }
}