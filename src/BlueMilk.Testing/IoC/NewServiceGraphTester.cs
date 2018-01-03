using System.Linq;
using BlueMilk.Codegen;
using BlueMilk.IoC.Instances;
using BlueMilk.Testing.TargetTypes;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC
{
    public class NewServiceGraphTester
    {
        public readonly ServiceRegistry theServices = new ServiceRegistry();

        public NewServiceGraphTester()
        {
            
        }
        
        [Fact]
        public void finds_the_single_default()
        {
            theServices.AddTransient<IWidget, AWidget>();
            var theGraph = new NewServiceGraph(theServices);

            theGraph.FindDefault(typeof(IWidget))
                .ShouldBeOfType<ConstructorInstance>()
                .ImplementationType.ShouldBe(typeof(AWidget));
        }

        [Fact]
        public void finds_the_last_as_the_default()
        {
            theServices.AddTransient<IWidget, AWidget>();
            theServices.AddSingleton(this);
            theServices.AddTransient<IGeneratedMethod, GeneratedMethod>();
            theServices.AddTransient<IWidget, MoneyWidget>();
            
            var theGraph = new NewServiceGraph(theServices);

            theGraph.FindDefault(typeof(IWidget))
                .ImplementationType.ShouldBe(typeof(MoneyWidget));

        }

        [Fact]
        public void finds_all()
        {
            theServices.AddTransient<IWidget, AWidget>();
            theServices.AddSingleton(this);
            theServices.AddTransient<IGeneratedMethod, GeneratedMethod>();
            theServices.AddTransient<IWidget, MoneyWidget>();
            
            var theGraph = new NewServiceGraph(theServices);

            theGraph.FindAll(typeof(IWidget))
                .Select(x => x.ImplementationType)
                .ShouldHaveTheSameElementsAs(typeof(AWidget), typeof(MoneyWidget));

        }

    }
}