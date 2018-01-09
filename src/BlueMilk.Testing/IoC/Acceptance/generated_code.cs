using System.Threading.Tasks;

namespace Jasper.Generated
{
    // START: BlueMilk_Testing_IoC_Acceptance_end_to_end_resolution_WidgetWithThing_WidgetWithThing
    public class BlueMilk_Testing_IoC_Acceptance_end_to_end_resolution_WidgetWithThing_WidgetWithThing : BlueMilk.IoC.Resolvers.TransientResolver<BlueMilk.Testing.TargetTypes.IWidget>
    {

        public BlueMilk_Testing_IoC_Acceptance_end_to_end_resolution_WidgetWithThing_WidgetWithThing()
        {
        }


        public override System.Object Build(BlueMilk.IoC.Scope scope)
        {
            var Thing = new BlueMilk.Testing.IoC.Acceptance.end_to_end_resolution.Thing();
            var WidgetWithThing = new BlueMilk.Testing.IoC.Acceptance.end_to_end_resolution.WidgetWithThing(Thing);
            return WidgetWithThing;
        }

    }

    // END: BlueMilk_Testing_IoC_Acceptance_end_to_end_resolution_WidgetWithThing_WidgetWithThing
    
    
    // START: BlueMilk_Testing_IoC_Acceptance_end_to_end_resolution_GuyWithWidget_GuyWithWidget
    public class BlueMilk_Testing_IoC_Acceptance_end_to_end_resolution_GuyWithWidget_GuyWithWidget : BlueMilk.IoC.Resolvers.TransientResolver<BlueMilk.Testing.IoC.Acceptance.end_to_end_resolution.GuyWithWidget>
    {

        public BlueMilk_Testing_IoC_Acceptance_end_to_end_resolution_GuyWithWidget_GuyWithWidget()
        {
        }


        public override System.Object Build(BlueMilk.IoC.Scope scope)
        {
            var Thing = new BlueMilk.Testing.IoC.Acceptance.end_to_end_resolution.Thing();
            var WidgetWithThing = new BlueMilk.Testing.IoC.Acceptance.end_to_end_resolution.WidgetWithThing(Thing);
            var GuyWithWidget = new BlueMilk.Testing.IoC.Acceptance.end_to_end_resolution.GuyWithWidget(WidgetWithThing);
            return GuyWithWidget;
        }

    }

    // END: BlueMilk_Testing_IoC_Acceptance_end_to_end_resolution_GuyWithWidget_GuyWithWidget
    
    
}