using System.Threading.Tasks;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.Methods;
using BlueMilk.Compilation;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.Codegen.Methods
{
    public class AsyncSingleReturnGeneratedMethodTester
    {
        [Fact]
        public async Task can_generate_method()
        {
            var assembly = new GeneratedAssembly(new GenerationRules("Jasper.Generated"));

            var method = AsyncSingleReturnGeneratedMethod.For<int>("GetNumber");
            var generatedType = assembly.AddType("NumberGetter", typeof(INumberGetter));
            generatedType.AddMethod(method);
            method.Frames.Add(new ReturnFive());
            
            assembly.CompileAll();

            var getter = generatedType.CreateInstance<INumberGetter>();

            var number = await getter.GetNumber();
            
            number.ShouldBe(5);
        }
    }

    public class ReturnFive : AsyncFrame
    {
        public override void GenerateCode(IGeneratedMethod method, ISourceWriter writer)
        {
            writer.Write("return Task.FromResult(5);");
        }
    }

    public interface INumberGetter
    {
        Task<int> GetNumber();
    }
}