using System;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC;
using BlueMilk.IoC.Instances;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Shouldly;
using StructureMap.Testing.Widget2;
using Xunit;

namespace BlueMilk.Testing.IoC.Instances
{
    public class ValueInstanceTester
    {
        private Variable variableFor<T>(T value)
        {
            var instance = ValueInstance.For(value);
            return instance.CreateVariable(BuildMode.Build, null, false);
        }
        
        [Fact]
        public void variable_for_string()
        {
            variableFor("Hello").Usage.ShouldBe("\"Hello\"");
        }
        
        [Fact]
        public void variable_for_numbers()
        {
            variableFor(22).Usage.ShouldBe("22");
            variableFor(22L).Usage.ShouldBe("22");
            variableFor(23.5).Usage.ShouldBe("23.5");
        }
        
        [Fact]
        public void variable_for_bool()
        {
            variableFor(true).Usage.ShouldBe("true");
            variableFor(false).Usage.ShouldBe("false");
        }
        
        [Fact]
        public void variable_for_enum()
        {
            variableFor(BreedEnum.Angus)
                .Usage.ShouldBe($"{typeof(BreedEnum).FullName}.Angus");
        }
        
        [Fact]
        public void variable_for_guid()
        {
            var guid = Guid.NewGuid();
            
            variableFor(guid).Usage.ShouldBe($"{typeof(Guid).FullName}.{nameof(Guid.Parse)}(\"{guid.ToString()}\")");
            
        }    
    }
}