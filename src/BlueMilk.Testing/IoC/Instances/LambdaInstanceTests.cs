using BlueMilk.IoC;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC.Instances
{
    public class LambdaInstanceTests
    {
        private readonly ServiceGraph theServices = ServiceGraph.Empty();
        
        [Fact]
        public void derive_the_default_name()
        {
            LambdaInstance.For(s => new Clock())
                .Name.ShouldBe(nameof(Clock));
        }
        
        [Fact]
        public void requires_service_provider()
        {
            LambdaInstance.For(s => new Clock())
                .RequiresServiceProvider.ShouldBeTrue();
        }
        

        [Fact]
        public void build_a_variable_returns_a_get_instance_frame()
        {
            var instance = LambdaInstance.For<IClock>(s => new Clock(), ServiceLifetime.Singleton);
            instance.CreateVariable(BuildMode.Inline, null, false)
                .Creator.ShouldBeOfType<GetInstanceFrame>();
        }
    }
}