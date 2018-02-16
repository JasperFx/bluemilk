using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using StructureMap.Testing.Widget;
using StructureMap.Testing.Widget3;
using Xunit;

namespace BlueMilk.Testing.IoC.Acceptance
{
    public class building_internal_types
    {
        [Fact]
        public void still_choose_greediest_constructor()
        {
            var container = new Container(_ =>
            {
                _.AddTransient<IWidget, AWidget>();
                _.AddTransient<IService, WhateverService>();

                _.AddTransient<IGadget, PrivateGadget>();
                _.AddTransient<PrivateGadgetHolder>();
            });

            var gadget = container.GetInstance<IGadget>()
                .ShouldBeOfType<PrivateGadget>();
            
            gadget
                .Widget.ShouldBeOfType<AWidget>();

            gadget.Service.ShouldBeOfType<WhateverService>();
            
            gadget.Clock.ShouldBeNull();
        }
    }

    public interface IGadget
    {
        
    }

    internal class PrivateGadgetHolder
    {
        public PrivateGadget Gadget { get; }

        public PrivateGadgetHolder(PrivateGadget gadget)
        {
            Gadget = gadget;
        }
    }

    internal class PrivateGadget : IGadget
    {
        public IClock Clock { get; private set; }
        public IService Service { get; }
        public IWidget Widget { get; }

        public PrivateGadget(IWidget widget)
        {
            Widget = widget;
        }

        public PrivateGadget(IWidget widget, IService service) : this(widget)
        {
            Service = service;
        }

        public PrivateGadget(IWidget widget, IService service, IClock clock)
            : this(widget, service)
        {
            Clock = clock;
        }
    }
}