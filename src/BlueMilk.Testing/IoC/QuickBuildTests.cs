using System;
using BlueMilk.IoC.Instances;
using BlueMilk.Testing.TargetTypes;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC
{
    public class QuickBuildTests
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