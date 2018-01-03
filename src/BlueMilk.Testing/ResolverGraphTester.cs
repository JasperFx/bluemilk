using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing
{
    public class ResolverGraphTester
    {
        [Fact]
        public void register_an_instance_that_is_not_the_default()
        {
            var graph = ResolverGraph.Empty();

            var instance = ObjectInstance.For(new Clock());
            instance.IsDefault = false;
            
            var resolver = Substitute.For<IResolver>();
            
            graph.Register(instance, resolver);
            
            graph.ByType.ContainsKey(instance.ServiceType).ShouldBeFalse();
            
            graph.ByTypeAndName[instance.ServiceType][instance.Name]
                .ShouldBeSameAs(resolver);


        }
        
        [Fact]
        public void register_an_instance_that_is_the_default()
        {
            var graph = ResolverGraph.Empty();

            var instance = ObjectInstance.For(new Clock());
            instance.IsDefault = true;
            
            var resolver = Substitute.For<IResolver>();
            
            graph.Register(instance, resolver);
            
            graph.ByType[instance.ServiceType].ShouldBeSameAs(resolver);
            
            graph.ByTypeAndName[instance.ServiceType][instance.Name]
                .ShouldBeSameAs(resolver);


        }
    }
}