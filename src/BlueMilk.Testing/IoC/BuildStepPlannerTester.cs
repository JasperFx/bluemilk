﻿using BlueMilk.Codegen;
using BlueMilk.Compilation;
using BlueMilk.IoC;
using BlueMilk.Testing.TargetTypes;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC
{
    public class CommentFrame : SyncFrame
    {
        public override void GenerateCode(IGeneratedMethod method, ISourceWriter writer)
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
            return new BuildStepPlanner(typeof(T), typeof(T), theGraph, theMethod);
        }

        [Fact]
        public void not_reduceable_if_no_public_ctors()
        {
            executePlan<NoPublicCtorGuy>()
                .CanBeReduced.ShouldBeFalse();
        }

        [Fact]
        public void not_reduceable_if_cannot_resolve_all()
        {
            executePlan<WidgetUsingGuy>()
                .CanBeReduced.ShouldBeFalse();
        }

        [Fact]
        public void reduceable_if_can_be_built_with_services()
        {
            theServices.AddTransient<IWidget, AWidget>();

            executePlan<WidgetUsingGuy>()
                .CanBeReduced.ShouldBeTrue();
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
