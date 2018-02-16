using System;
using System.Reflection;
using Baseline;
using BlueMilk.IoC.Instances;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC.Instances
{
    public class CtorFuncBuilderTester
    {
        [Fact]
        public void try_to_build_simple_constructors()
        {
            var constructors = typeof(Gadget).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            (var func1, var funcType1) = CtorFuncBuilder.LambdaTypeFor(typeof(Gadget),constructors[0]);
            (var func2, var funcType2) = CtorFuncBuilder.LambdaTypeFor(typeof(Gadget),constructors[1]);
            (var func3, var funcType3) = CtorFuncBuilder.LambdaTypeFor(typeof(Gadget),constructors[2]);

            func1.As<Func<Gadget>>()().ShouldBeOfType<Gadget>();
            func2.As<Func<string, Gadget>>()("Jon").Name.ShouldBe("Jon");
            func3.As<Func<string, int, Gadget>>()("Jon", 15).Age.ShouldBe(15);
        }
    }

    internal class Gadget
    {
        public string Name { get; }
        public int Age { get; }

        public Gadget()
        {
        }

        internal Gadget(string name)
        {
            Name = name;
        }

        public Gadget(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}