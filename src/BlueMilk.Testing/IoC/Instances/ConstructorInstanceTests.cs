using System.Linq;
using BlueMilk.Codegen;
using BlueMilk.IoC.Instances;
using BlueMilk.Testing.TargetTypes;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC.Instances
{
    public class ConstructorInstanceTests
    {
        [Fact]
        public void derive_the_default_name()
        {
            ConstructorInstance.For<IClock, DisposableClock>()
                .Name.ShouldBe(nameof(DisposableClock));

        }
        
        [Fact]
        public void select_greediest_constructor_that_can_be_filled()
        {
            var theServices = new ServiceRegistry();
            theServices.AddTransient<IWidget, AWidget>();
            theServices.AddSingleton(this);
            theServices.AddTransient<IGeneratedMethod, GeneratedMethod>();
            theServices.AddTransient<IWidget, MoneyWidget>();
            
            var theGraph = new NewServiceGraph(theServices);

            var instance = ConstructorInstance.For<DeepConstructorGuy>();
            
            instance.CreatePlan(theGraph);


            instance.Constructor.GetParameters().Select(x => x.ParameterType)
                .ShouldHaveTheSameElementsAs(typeof(IWidget), typeof(IGeneratedMethod));
        }
        
        [Fact]
        public void will_choose_a_no_arg_ctor_if_that_is_all_there_is()
        {
            var theServices = new ServiceRegistry();
            var theGraph = new NewServiceGraph(theServices);

            var instance = ConstructorInstance.For<NoArgGuy>();
            instance.CreatePlan(theGraph);
            
            instance.Constructor.ShouldNotBeNull();
            instance.Constructor.GetParameters().Any().ShouldBeFalse();
        }
        
        [Fact]
        public void happy_path_can_find_ctor_no_error_messages()
        {
            var theServices = new ServiceRegistry();
            var theGraph = new NewServiceGraph(theServices);

            var instance = ConstructorInstance.For<NoArgGuy>();
            instance.CreatePlan(theGraph);
            
            instance.ErrorMessages.Any().ShouldBeFalse();
        }
        
        [Fact]
        public void add_error_message_if_no_public_constructors()
        {
            var theServices = new ServiceRegistry();
            var theGraph = new NewServiceGraph(theServices);

            var instance = ConstructorInstance.For<GuyWithNoPublicConstructors>();
            instance.CreatePlan(theGraph);
            
            instance.Constructor.ShouldBeNull();
            instance.ErrorMessages.ShouldContain(ConstructorInstance.NoPublicConstructors);
        }
        
        [Fact]
        public void add_error_message_if_no_public_ctor_can_be_filled()
        {
            var theServices = new ServiceRegistry();

            var theGraph = new NewServiceGraph(theServices);

            var instance = ConstructorInstance.For<GuyThatUsesIWidget>();
            
            instance.CreatePlan(theGraph);
            
            instance.Constructor.ShouldBeNull();
            instance.ErrorMessages.ShouldContain(ConstructorInstance.NoPublicConstructorCanBeFilled);
        }
        
        /*
         * TODO's

         * 3. Find all dependent instances
         * 4. Select the creation style if not singleton
         * 5. Select the creation style if singleton, and no dependencies on Lambdas
         * 6. Select the creation style if singleton, but there's a dependency on a Lambda
         * 7. Choose base type
         *    a.) no arg transient
         *    b.) no arg scoped
         *    c.) no arg singleton
         *    d.) transient
         *    e.) scoped
         *    f.) singleton
         */
    }

    public class GuyThatUsesIWidget
    {
        public GuyThatUsesIWidget(IWidget widget)
        {
        }
    }
    
    public class GuyWithNoPublicConstructors
    {
        
        
        private GuyWithNoPublicConstructors()
        {
            
        }
    }

}