using System.Collections.Generic;
using BlueMilk.Codegen;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC
{
    public class KnownVariableBuildStep : BuildStep
    {
        public new Variable Variable { get; }

        public KnownVariableBuildStep(Variable variable) : base(variable.VariableType, false, ServiceLifetime.Scoped)
        {
            Variable = variable;
        }

        public override IEnumerable<BuildStep> ReadDependencies(BuildStepPlanner planner)
        {
            yield break;
        }

        protected override Variable buildVariable()
        {
            return Variable;
        }
    }
}
