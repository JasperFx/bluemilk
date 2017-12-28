using BlueMilk.Codegen;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.Codegen
{
    public class ReflectionExtensionsTests
    {
        [Fact]
        public void get_full_name_in_code_for_generic_type()
        {
            typeof(Handler<Message1>).FullNameInCode()
                .ShouldBe($"BlueMilk.Testing.Codegen.Handler<{typeof(Message1).FullName}>");
        }
    }

    public class Handler<T>
    {

    }

    public class Message1{}
}
