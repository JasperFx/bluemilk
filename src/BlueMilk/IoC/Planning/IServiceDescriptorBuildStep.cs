using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Planning
{
    public interface IServiceDescriptorBuildStep
    {
        ServiceDescriptor ServiceDescriptor { get; }
    }
}
