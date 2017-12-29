using System;
using System.Collections.Generic;
using BlueMilk.Codegen;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC
{
    public enum BuildMode
    {
        Inline,
        ContainerActivation,
        Resolver
    }


    
    public abstract class BuildStep
    {
        private readonly Lazy<Variable> _variable;
        public Type ServiceType { get; }

        public BuildStep(Type serviceType, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;

            _variable = new Lazy<Variable>(buildVariable);
        }

        public ServiceLifetime Lifetime { get; internal set; }

        /// <summary>
        /// If you are creating multiple instances of the same concrete type, use
        /// this as a suffix on the variable
        /// </summary>
        // TODO -- think this will need to be moved up to the generated method scope and be
        // on Variable itself
        public int Number { get; set; }

        public abstract IEnumerable<BuildStep> ReadDependencies(BuildStepPlanner planner);

        protected abstract Variable buildVariable();

        public Variable Variable => _variable.Value;
    }
}
