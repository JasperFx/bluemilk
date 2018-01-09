using BlueMilk.Codegen;
using BlueMilk.Codegen.Frames;
using BlueMilk.Compilation;
using BlueMilk.IoC.Planning;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using StructureMap.Testing.Widget;
using Xunit;

namespace BlueMilk.Testing.IoC
{
    public class CommentFrame : SyncFrame
    {
        public override void GenerateCode(GeneratedMethod method, ISourceWriter writer)
        {
            writer.Write("// Kilroy was here.");
            Next?.GenerateCode(method, writer);
        }
    }

    public class BuildStepPlannerTester
    {
        public readonly ServiceRegistry theServices = new ServiceRegistry();
        private ServiceGraph theGraph;
        private StubMethodVariables theMethod = new StubMethodVariables();

        public BuildStepPlannerTester()
        {
            theGraph = new ServiceGraph(theServices);
        }

        private BuildStepPlanner executePlan<T>()
        {
            var planner = new BuildStepPlanner(theGraph, theMethod);
            var top = planner.PlanConcreteBuild(typeof(T));

            return planner;
        }

        [Fact]
        public void not_reduceable_if_no_public_ctors()
        {
            executePlan<NoPublicCtorGuy>()
                .Determination.ShouldBe(PlanningDetermination.Missing);
        }

        [Fact]
        public void not_reduceable_if_cannot_resolve_all()
        {
            executePlan<WidgetUsingGuy>()
                .Determination.ShouldBe(PlanningDetermination.Missing);
        }

        [Fact]
        public void reduceable_if_can_be_built_with_services()
        {
            theServices.AddTransient<IWidget, AWidget>();

            executePlan<WidgetUsingGuy>()
                .Determination.ShouldBe(PlanningDetermination.ConstructorsOnly);
        }

    }

    public class WidgetUsingGuy
    {
        public WidgetUsingGuy(IWidget widget)
        {
        }
    }

    public class NoPublicCtorGuy
    {
        private NoPublicCtorGuy()
        {
        }
    }
}
