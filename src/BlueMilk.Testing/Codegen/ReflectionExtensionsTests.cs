using System;
using System.Threading.Tasks;
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

        public interface ISomeInterface<T>
        {
            
        }
        
        [Fact]
        public void get_full_name_in_code_for_inner_generic_type()
        {
            typeof(ISomeInterface<string>).FullNameInCode()
                .ShouldBe("BlueMilk.Testing.Codegen.ReflectionExtensionsTests.ISomeInterface<string>");
        }
        
        [Fact]
        public void get_name_in_code_for_inner_generic_type()
        {
            typeof(ISomeInterface<string>).NameInCode()
                .ShouldBe("ReflectionExtensionsTests.ISomeInterface<string>");
        }
        
        [Theory]
        [InlineData(typeof(void), "void")]
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(string), "string")]
        [InlineData(typeof(long), "long")]
        [InlineData(typeof(bool), "bool")]
        [InlineData(typeof(double), "double")]
        [InlineData(typeof(object), "object")]
        [InlineData(typeof(Message1), "Message1")]
        [InlineData(typeof(Handler<Message1>), "Handler<BlueMilk.Testing.Codegen.Message1>")]
        [InlineData(typeof(Handler<string>), "Handler<string>")]
        public void alias_name_of_task(Type type, string name)
        {
            type.NameInCode().ShouldBe(name);
        }
        
        [Theory]
        [InlineData(typeof(void), "void")]
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(string), "string")]
        [InlineData(typeof(long), "long")]
        [InlineData(typeof(bool), "bool")]
        [InlineData(typeof(double), "double")]
        [InlineData(typeof(object), "object")]
        [InlineData(typeof(Message1), "BlueMilk.Testing.Codegen.Message1")]
        [InlineData(typeof(Handler<Message1>), "BlueMilk.Testing.Codegen.Handler<BlueMilk.Testing.Codegen.Message1>")]
        [InlineData(typeof(Handler<string>), "BlueMilk.Testing.Codegen.Handler<string>")]
        public void alias_full_name_of_task(Type type, string name)
        {
            type.FullNameInCode().ShouldBe(name);
        }
        
        [Fact]
        public void name_in_code_of_inner_type()
        {
            typeof(ThingHolder.Thing1).NameInCode().ShouldBe("ThingHolder.Thing1");
        }
    }

    public class ThingHolder
    {
        public class Thing1
        {
            
        }
    }

    public class Handler<T>
    {

    }

    public class Message1{}
}
