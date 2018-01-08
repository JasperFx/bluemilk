using System;
using System.Composition.Hosting.Core;
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
            
            var theGraph = new NewServiceGraph(theServices, Scope.Empty());

            var instance = ConstructorInstance.For<DeepConstructorGuy>();
            
            instance.CreatePlan(theGraph);


            instance.Constructor.GetParameters().Select(x => x.ParameterType)
                .ShouldHaveTheSameElementsAs(typeof(IWidget), typeof(GeneratedMethod));
        }
        
        [Fact]
        public void will_choose_a_no_arg_ctor_if_that_is_all_there_is()
        {
            var theServices = new ServiceRegistry();
            var theGraph = new NewServiceGraph(theServices, Scope.Empty());

            var instance = ConstructorInstance.For<NoArgGuy>();
            instance.CreatePlan(theGraph);
            
            instance.Constructor.ShouldNotBeNull();
            instance.Constructor.GetParameters().Any().ShouldBeFalse();
        }
        
        [Fact]
        public void happy_path_can_find_ctor_no_error_messages()
        {
            var theServices = new ServiceRegistry();
            var theGraph = new NewServiceGraph(theServices, Scope.Empty());

            var instance = ConstructorInstance.For<NoArgGuy>();
            instance.CreatePlan(theGraph);
            
            instance.ErrorMessages.Any().ShouldBeFalse();
        }
        
        [Fact]
        public void add_error_message_if_no_public_constructors()
        {
            var theServices = new ServiceRegistry();
            var theGraph = new NewServiceGraph(theServices, Scope.Empty());

            var instance = ConstructorInstance.For<GuyWithNoPublicConstructors>();
            instance.CreatePlan(theGraph);
            
            instance.Constructor.ShouldBeNull();
            instance.ErrorMessages.ShouldContain(ConstructorInstance.NoPublicConstructors);
        }
        
        [Fact]
        public void add_error_message_if_no_public_ctor_can_be_filled()
        {
            var theServices = new ServiceRegistry();

            var theGraph = new NewServiceGraph(theServices, Scope.Empty());

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
            
            var theGraph = new NewServiceGraph(theServices, Scope.Empty());
            var instance = ConstructorInstance.For<GuyWithWidgetAndRule>();
            
            instance.CreatePlan(theGraph);
            
            instance.Dependencies.OfType<ConstructorInstance>()
                .Select(x => x.ImplementationType)
                .ShouldBe(new []{typeof(AWidget), typeof(BlueRule)});
        }
        
        public class GuyWithGuys
        {
            public GuyWithGuys(GuyWithWidgetAndRule guy1, OtherGuy other)
            {
            }
        }

        public class OtherGuy
        {
        
        }

        [Fact]
        public void find_dependencies_deep()
        {
            var theServices = new ServiceRegistry();
            theServices.AddSingleton<IWidget, AWidget>();
            theServices.AddTransient<Rule, BlueRule>();
            theServices.AddTransient<OtherGuy>();
            theServices.AddTransient<GuyWithWidgetAndRule>();

            var theGraph = new NewServiceGraph(theServices, Scope.Empty());
            var instance = ConstructorInstance.For<GuyWithGuys>();

            instance.CreatePlan(theGraph);

            var expected = new[]
            {
                typeof(AWidget), 
                typeof(BlueRule),
                typeof(OtherGuy), 
                typeof(GuyWithWidgetAndRule)
            };


        instance.Dependencies.OfType<ConstructorInstance>()
                .Select(x => x.ImplementationType)
                .ShouldBe(expected, true);

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
            instance.RequiresServiceProvider.ShouldBeFalse();
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
            instance.RequiresServiceProvider.ShouldBeFalse();
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
            instance.RequiresServiceProvider.ShouldBeFalse();
        }
        
        [Fact]
        public void requires_service_provider_with_dependencies_negative()
        {
            var theServices = new ServiceRegistry();
            theServices.AddSingleton<IWidget, AWidget>();
            theServices.AddTransient<Rule, BlueRule>();
            
            var theGraph = new NewServiceGraph(theServices, Scope.Empty());
            var instance = ConstructorInstance.For<GuyWithWidgetAndRule>();
            
            instance.CreatePlan(theGraph);
            
            instance.RequiresServiceProvider.ShouldBeFalse();
        }
        
        [Fact]
        public void requires_service_provider_with_dependencies_positive()
        {
            var theServices = new ServiceRegistry();
            theServices.AddSingleton<IWidget, AWidget>();
            theServices.AddTransient<Rule>(x => new BlueRule());
            
            var theGraph = new NewServiceGraph(theServices, Scope.Empty());
            var instance = ConstructorInstance.For<GuyWithWidgetAndRule>();
            
            instance.CreatePlan(theGraph);
            
            instance.RequiresServiceProvider.ShouldBeTrue();
        }
        
        [Fact]
        public void singleton_can_be_inlined_if_no_service_provider_requirements()
        {
            var theServices = new ServiceRegistry();
            theServices.AddSingleton<IWidget, AWidget>();
            theServices.AddTransient<Rule, BlueRule>();
            
            var theGraph = new NewServiceGraph(theServices, Scope.Empty());
            var instance = ConstructorInstance.For<GuyWithWidgetAndRule>();
            instance.Lifetime = ServiceLifetime.Singleton;
            
            instance.CreatePlan(theGraph);
            
            instance.CreationStyle.ShouldBe(CreationStyle.InlineSingleton);
        }
        
        [Fact]
        public void singleton_can_not_be_inlined_if_there_are_service_provider_requirements()
        {
            var theServices = new ServiceRegistry();
            theServices.AddSingleton<IWidget, AWidget>();
            theServices.AddTransient<Rule>(x => new BlueRule());
            
            var theGraph = new NewServiceGraph(theServices, Scope.Empty());
            var instance = ConstructorInstance.For<GuyWithWidgetAndRule>();
            instance.Lifetime = ServiceLifetime.Singleton;
            
            instance.CreatePlan(theGraph);
            
            instance.CreationStyle.ShouldBe(CreationStyle.Generated);
        }
        
        [Theory]
        [InlineData(ServiceLifetime.Transient, typeof(TransientResolver<>))]
        [InlineData(ServiceLifetime.Singleton, typeof(SingletonResolver<>))]
        [InlineData(ServiceLifetime.Scoped, typeof(ScopedResolver<>))]
        public void choose_base_type_of_resolver(ServiceLifetime lifetime, Type baseType)
        {
            var theServices = new ServiceRegistry();
            theServices.AddSingleton<IWidget, AWidget>();
            theServices.AddTransient<Rule>(x => new BlueRule());
            
            var theGraph = new NewServiceGraph(theServices, Scope.Empty());
            var instance = ConstructorInstance.For<GuyWithWidgetAndRule>();
            instance.Lifetime = lifetime;
            
            instance.CreatePlan(theGraph);
            
            instance.ResolverBaseType.ShouldBe(baseType);
        }

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

    public class WidgetWithRule : IWidget
    {
        public Rule Rule { get; }

        public WidgetWithRule(Rule rule)
        {
            Rule = rule;
        }

        public void DoSomething()
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