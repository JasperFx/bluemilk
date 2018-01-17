using BlueMilk.Compilation;

namespace BlueMilk.IoC.Instances
{
    public interface IInstanceThatGeneratesResolver
    {
        void GenerateResolver(GeneratedAssembly generatedAssembly);
    }
}