using System;
using BlueMilk.IoC.Instances;
using BlueMilk.Testing.IoC.Acceptance;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using StructureMap.Testing.Widget;
using Xunit;

namespace BlueMilk.Testing.IoC
{
    public class quick_build_specs
    {
        [Fact]
        public void happy_path()
        {
            var clock = new Clock();
            var widget = new AWidget();
            
            var container = Container.For(_ =>
            {
                _.AddSingleton<IClock>(clock);
                _.AddSingleton<IWidget>(widget);
            });

            var user = container.QuickBuild<WidgetUser>();
            user.Clock.ShouldBeSameAs(clock);
            user.Widget.ShouldBeSameAs(widget);

        }
        
        [Fact]
        public void uses_greediest_constructor_it_can()
        {
            var clock = new Clock();
            var widget = new AWidget();
            
            var container = Container.For(_ =>
            {
                _.AddSingleton<IClock>(clock);
                //_.AddSingleton<IWidget>(widget);
            });
            
            var user = container.QuickBuild<WidgetUser>();
            user.Clock.ShouldBeSameAs(clock);
            user.Widget.ShouldBeNull();
        }
        
        [Fact]
        public void throw_if_no_suitable_constructor()
        {
            var clock = new Clock();
            var widget = new AWidget();
            
            var container = Container.For(_ =>
            {
                _.AddSingleton<IClock>(clock);
                _.AddSingleton<IWidget>(widget);
            });

            Exception<InvalidOperationException>.ShouldBeThrownBy(() =>
            {
                container.QuickBuild<GuyWithStringArg>();
            }).Message.ShouldContain(ConstructorInstance.NoPublicConstructorCanBeFilled);
        }
        
        [Fact]
        public void throw_if_no_public_constructors()
        {
            var clock = new Clock();
            var widget = new AWidget();
            
            var container = Container.For(_ =>
            {
                _.AddSingleton<IClock>(clock);
                _.AddSingleton<IWidget>(widget);
            });

            Exception<InvalidOperationException>.ShouldBeThrownBy(() =>
            {
                container.QuickBuild<GuyWithNoPublicConstructors>();
            }).Message.ShouldContain(ConstructorInstance.NoPublicConstructors);
        }
        
        [Fact]
        public void throw_if_not_concrete()
        {
            var clock = new Clock();
            var widget = new AWidget();
            
            var container = Container.For(_ =>
            {
                _.AddSingleton<IClock>(clock);
                _.AddSingleton<IWidget>(widget);
            });

            Exception<InvalidOperationException>.ShouldBeThrownBy(() => { container.QuickBuild<IWidget>(); });
        }

        public class GreenWidget : IWidget
        {
            public void DoSomething()
            {
                throw new NotImplementedException();
            }
        }

        public class BlueWidget : IWidget
        {
            public void DoSomething()
            {
                throw new NotImplementedException();
            }
        }
        
        [Fact]
        public void honor_named_attribute_on_parameter()
        {
            var container = new Container(_ =>
            {
                _.For<IWidget>().Use<GreenWidget>().Named("green");
                _.For<IWidget>().Use<BlueWidget>().Named("blue");
            });

            container.GetInstance<IWidget>().ShouldBeOfType<BlueWidget>();

            container.QuickBuild<SelectiveWidgetUser>()
                .Widget.ShouldBeOfType<GreenWidget>();
        }
    }

    public class GuyWithNoPublicConstructors
    {
        private GuyWithNoPublicConstructors()
        {
        }
    }

    public class GuyWithStringArg
    {
        public GuyWithStringArg(IWidget widget, string arg)
        {
        }
    }

    public class SelectiveWidgetUser
    {
        public IWidget Widget { get; }

        public SelectiveWidgetUser([Named("green")]IWidget widget)
        {
            Widget = widget;
        }
    }
    
    public class WidgetUser
    {
        public IWidget Widget { get; }
        public IClock Clock { get; }

        public WidgetUser(IWidget widget, IClock clock)
        {
            Widget = widget;
            Clock = clock;
        }

        public WidgetUser(IClock clock)
        {
            Clock = clock;
        }
    }
    
}