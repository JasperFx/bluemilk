using System.Linq;
using BlueMilk.Codegen;
using BlueMilk.IoC;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
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
            theServices.AddTransient<GeneratedMethod, GeneratedMethod>();
            theServices.AddTransient<IWidget, MoneyWidget>();
            
            var theGraph = new NewServiceGraph(theServices, new Scope());

            var instance = ConstructorInstance.For<DeepConstructorGuy>();
            
            instance.CreatePlan(theGraph);


            instance.Constructor.GetParameters().Select(x => x.ParameterType)
                .ShouldHaveTheSameElementsAs(typeof(IWidget), typeof(GeneratedMethod));
        }
        
        [Fact]
        public void will_choose_a_no_arg_ctor_if_that_is_all_there_is()
        {
            var theServices = new ServiceRegistry();
            var theGraph = new NewServiceGraph(theServices, new Scope());

            var instance = ConstructorInstance.For<NoArgGuy>();
            instance.CreatePlan(theGraph);
            
            instance.Constructor.ShouldNotBeNull();
            instance.Constructor.GetParameters().Any().ShouldBeFalse();
        }
        
        [Fact]
        public void happy_path_can_find_ctor_no_error_messages()
        {
            var theServices = new ServiceRegistry();
            var theGraph = new NewServiceGraph(theServices, new Scope());

            var instance = ConstructorInstance.For<NoArgGuy>();
            instance.CreatePlan(theGraph);
            
            instance.ErrorMessages.Any().ShouldBeFalse();
        }
        
        [Fact]
        public void add_error_message_if_no_public_constructors()
        {
            var theServices = new ServiceRegistry();
            var theGraph = new NewServiceGraph(theServices, new Scope());

            var instance = ConstructorInstance.For<GuyWithNoPublicConstructors>();
            instance.CreatePlan(theGraph);
            
            instance.Constructor.ShouldBeNull();
            instance.ErrorMessages.ShouldContain(ConstructorInstance.NoPublicConstructors);
        }
        
        [Fact]
        public void add_error_message_if_no_public_ctor_can_be_filled()
        {
            var theServices = new ServiceRegistry();

            var theGraph = new NewServiceGraph(theServices, new Scope());

            var instance = ConstructorInstance.For<GuyThatUsesIWidget>();
            
            instance.CreatePlan(theGraph);
            
            instance.Constructor.ShouldBeNull();
            instance.ErrorMessages.ShouldContain(ConstructorInstance.NoPublicConstructorCanBeFilled);
        }
        
        [Fact]
        public void find_dependencies()
        {
            var theServices = new ServiceRegistry();
            theServices.AddSingleton<IWidget, AWidget>();
            theServices.AddTransient<Rule, BlueRule>();
            
            var theGraph = new NewServiceGraph(theServices, new Scope());
            var instance = ConstructorInstance.For<GuyWithWidgetAndRule>();
            
            instance.CreatePlan(theGraph);
            
            instance.Dependencies.OfType<ConstructorInstance>()
                .Select(x => x.ImplementationType)
                .ShouldBe(new []{typeof(AWidget), typeof(BlueRule)});
        }
        
        [Fact]
        public void creation_style_is_no_arg_for_transient()
        {
            var instance = ConstructorInstance.For<AWidget>();
            instance.Lifetime = ServiceLifetime.Transient;
            
            instance.CreatePlan(NewServiceGraph.Empty());
            
            instance.CreationStyle.ShouldBe(CreationStyle.NoArg);

            instance.BuildResolver(null, null).ShouldBeOfType<NoArgTransientResolver<AWidget>>();
            instance.ResolverBaseType.ShouldBeNull();
        }
        
        [Fact]
        public void creation_style_is_no_arg_for_scoped()
        {
            var instance = ConstructorInstance.For<AWidget>();
            instance.Lifetime = ServiceLifetime.Scoped;
            
            instance.CreatePlan(NewServiceGraph.Empty());
            
            instance.CreationStyle.ShouldBe(CreationStyle.NoArg);

            instance.BuildResolver(null, null).ShouldBeOfType<NoArgScopedResolver<AWidget>>();
            instance.ResolverBaseType.ShouldBeNull();
        }
        
        [Fact]
        public void creation_style_is_no_arg_for_singleton()
        {
            var instance = ConstructorInstance.For<AWidget>();
            instance.Lifetime = ServiceLifetime.Singleton;
            
            instance.CreatePlan(NewServiceGraph.Empty());
            
            instance.CreationStyle.ShouldBe(CreationStyle.InlineSingleton);

            instance.BuildResolver(null, null).ShouldBeNull();
            instance.ResolverBaseType.ShouldBeNull();
        }
        

        
        /*
         * TODO's


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

    public class GuyWithWidgetAndRule
    {
        public IWidget Widget { get; }
        public Rule Rule { get; }

        public GuyWithWidgetAndRule(IWidget widget, Rule rule)
        {
            Widget = widget;
            Rule = rule;
        }
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