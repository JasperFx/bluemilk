using System;
using System.Collections.Generic;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Planning
{
    public enum BuildMode
    {
        Inline,               // using blocks for disposables
        ContainerActivation,  // register with the scope for disposables. If singleton, register
        Resolver              // register with the scope for disposables
    }


    
    public abstract class BuildStep
    {
        public Type ServiceType { get; }

        public BuildStep(Type serviceType, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;

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

        protected abstract Variable buildVariable(BuildMode mode);
        private Variable _variable;

        private BuildMode? _mode;
        public Variable CreateVariable(BuildMode mode)
        {
            if (_mode != null && _mode != mode)
            {
                throw new ArgumentOutOfRangeException(nameof(mode), "Cannot change build modes!");
            }
            
            _mode = mode;

            if (Lifetime == ServiceLifetime.Transient)
            {
                return buildVariable(mode);
            }

            _variable = _variable ?? buildVariable(mode);

            return _variable;
        }
        
        public BuildStep[] Dependencies { get; internal set; }
    }
}
