using System.Linq;
using BlueMilk.Codegen;
using BlueMilk.IoC;
using BlueMilk.IoC.Instances;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using StructureMap.Testing.Widget;
using Xunit;

namespace BlueMilk.Testing.IoC
{
    public class ServiceGraphTester
    {

        public readonly ServiceRegistry theServices = new ServiceRegistry();

        public ServiceGraphTester()
        {
            
        }
        
        [Fact]
        public void finds_the_single_default()
        {
            theServices.AddTransient<IWidget, AWidget>();
            var theGraph = new ServiceGraph(theServices, Scope.Empty());
            theGraph.Initialize();

            theGraph.FindDefault(typeof(IWidget))
                .ShouldBeOfType<ConstructorInstance>()
                .ImplementationType.ShouldBe(typeof(AWidget));
        }

        [Fact]
        public void finds_the_last_as_the_default()
        {
            theServices.AddTransient<IWidget, AWidget>();
            theServices.AddSingleton(this);
            theServices.AddTransient<IThing, Thing>();
            theServices.AddTransient<IWidget, MoneyWidget>();
            
            var theGraph = new ServiceGraph(theServices, Scope.Empty());
            theGraph.Initialize();

            theGraph.FindDefault(typeof(IWidget))
                .ShouldBeOfType<ConstructorInstance>()
                .ImplementationType.ShouldBe(typeof(MoneyWidget));

        }

        [Fact]
        public void finds_all()
        {
            theServices.AddTransient<IWidget, AWidget>();
            theServices.AddSingleton(this);
            theServices.AddTransient<IThing, Thing>();
            theServices.AddTransient<IWidget, MoneyWidget>();
            
            var theGraph = new ServiceGraph(theServices, Scope.Empty());
            theGraph.Initialize();

            theGraph.FindAll(typeof(IWidget))
                .OfType<ConstructorInstance>()
                .Select(x => x.ImplementationType)
                .ShouldHaveTheSameElementsAs(typeof(AWidget), typeof(MoneyWidget));

        }

    }
}