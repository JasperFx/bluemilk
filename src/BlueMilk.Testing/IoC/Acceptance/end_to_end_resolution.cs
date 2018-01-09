using BlueMilk.Testing.TargetTypes;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC.Acceptance
{
    public class end_to_end_resolution
    {
        [Fact]
        public void two_deep_transient()
        {
            var container = Container.For(_ =>
            {
                _.For<IWidget>().Use<WidgetWithThing>();
                _.For<IThing>().Use<Thing>();
                _.AddTransient<GuyWithWidget>();
            });

            container.GetInstance<GuyWithWidget>()
                .Widget.ShouldBeOfType<WidgetWithThing>()
                .Thing.ShouldBeOfType<Thing>();
        }
        
        [Fact]
        public void resolve_singletons_via_constructor()
        {
            var container = Container.For(_ =>
            {
                _.For<IWidget>().Use<WidgetWithThing>();
                _.For<IThing>().Use<Thing>();
                _.AddSingleton<GuyWithWidget>();
            });

            container.GetInstance<GuyWithWidget>()
                .Widget.ShouldBeOfType<WidgetWithThing>()
                .Thing.ShouldBeOfType<Thing>();
        }

        public class GuyWithWidget
        {
            public IWidget Widget { get; }

            public GuyWithWidget(IWidget widget)
            {
                Widget = widget;
            }
        }
        
        public interface IThing{}
        public class Thing : IThing{}

        public class WidgetWithThing : IWidget
        {
            public IThing Thing { get; }

            public WidgetWithThing(IThing thing)
            {
                Thing = thing;
            }

            public void DoSomething()
            {
                
            }
        }
    }
}