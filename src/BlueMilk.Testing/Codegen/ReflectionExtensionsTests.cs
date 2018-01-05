﻿using System;
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
        
        [Theory]
        [InlineData(typeof(void), "void")]
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(string), "string")]
        [InlineData(typeof(long), "long")]
        [InlineData(typeof(bool), "bool")]
        [InlineData(typeof(double), "double")]
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
        [InlineData(typeof(Message1), "BlueMilk.Testing.Codegen.Message1")]
        [InlineData(typeof(Handler<Message1>), "BlueMilk.Testing.Codegen.Handler<BlueMilk.Testing.Codegen.Message1>")]
        [InlineData(typeof(Handler<string>), "BlueMilk.Testing.Codegen.Handler<string>")]
        public void alias_full_name_of_task(Type type, string name)
        {
            type.FullNameInCode().ShouldBe(name);
        }
    }

    public class Handler<T>
    {

    }

    public class Message1{}
}
