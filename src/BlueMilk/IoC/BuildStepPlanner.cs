using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Baseline;
using BlueMilk.Codegen;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC
{
    public enum PlanningDetermination
    {
        ConstructorsOnly,
        RequiresServiceProvider,
        Missing
    }
    
    
    public class BuildStepPlanner
    {
        private readonly ServiceGraph _graph;
        private readonly IMethodVariables _method;
        private readonly IList<BuildStep> _visited = new List<BuildStep>();
        private readonly Stack<BuildStep> _chain = new Stack<BuildStep>();
        
        private readonly List<string> _errorMessages = new List<string>();

        public BuildStepPlanner(ServiceGraph graph, IMethodVariables method)
        {
            _graph = graph;
            _method = method;

        }

        public IReadOnlyList<string> ErrorMessages => _errorMessages;

        public ConstructorBuildStep PlanConcreteBuild(Type concreteType)
        {
            return PlanConcreteBuild(concreteType, concreteType);
        }

        public ConstructorBuildStep PlanConcreteBuild(Type serviceType, Type concreteType)
        {
            if (!concreteType.IsConcrete()) throw new ArgumentOutOfRangeException(nameof(concreteType), "Must be a concrete type");
            
            var ctor = _graph.ChooseConstructor(concreteType);
            if (ctor == null)
            {
                _errorMessages.Add($"Could not determine a suitable, public constructor for concrete type {concreteType.FullNameInCode()}");
                Determination = PlanningDetermination.Missing;
                return null;
            }
            
            var step = new ConstructorBuildStep(serviceType, concreteType, ServiceLifetime.Scoped, ctor);
            Visit(step);

            return step;
        }

        public ConstructorBuildStep PlanConcreteBuild(ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationType == null || !descriptor.ImplementationType.IsConcrete())
            {
                throw new ArgumentOutOfRangeException(nameof(descriptor), $"ServiceDescriptor must specify a concrete {nameof(ServiceDescriptor.ImplementationType)}");
            }
            
            var step = PlanConcreteBuild(descriptor.ServiceType, descriptor.ImplementationType);
            if (step != null) step.Lifetime = descriptor.Lifetime;

            return step;
        }

        public PlanningDetermination Determination { get; private set; } = PlanningDetermination.ConstructorsOnly;

        public void Visit(BuildStep step)
        {
            if (_chain.Contains(step))
            {
                throw new InvalidOperationException("Bi-directional dependencies detected:" + Environment.NewLine + _chain.Select(x => x.ToString()).Join(Environment.NewLine));
            }

            if (_visited.Contains(step))
            {
                return;
            }

            _chain.Push(step);

            foreach (var dep in step.ReadDependencies(this))
            {
                // TODO -- have this check for a "MissingStep" instead of a null
                if (dep == null)
                {
                    Determination = PlanningDetermination.Missing;
                    return;
                }


                Visit(dep);
            }

            _chain.Pop();
        }

        public BuildStep FindStep(Type type)
        {
            try
            {
                var candidate = _method.AllKnownBuildSteps.FirstOrDefault(x => x.ServiceType == type);
                if (candidate != null) return candidate;

                var step = findStep(type);

                if (step != null)
                {
                    _method.AllKnownBuildSteps.Add(step);
                }

                return step;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Could not determine a BuildStep for '{type.FullName}'", e);
            }
        }

        public BuildStep FindStep(ServiceDescriptor descriptor)
        {
            var candidate = _method.AllKnownBuildSteps.OfType<IServiceDescriptorBuildStep>()
                .FirstOrDefault(x => x.ServiceDescriptor == descriptor);
            if (candidate != null) return candidate as BuildStep;

            var step = findStep(descriptor);
            if (step != null)
            {
                _method.AllKnownBuildSteps.Add(step);
            }

            return step;
        }

        private BuildStep findStep(Type type)
        {
            // INSTEAD, let's pull all variable sources
            // If not a ServiceVariable, use the KnownVariableBuildStep, otherwise use the
            // parent build step and do NOT visit its dependencies
            var variable = _method.TryFindVariable(type, VariableSource.NotServices);

            if (variable != null) return new KnownVariableBuildStep(variable);

            var @default = _graph.FindDefault(type);

            if (@default == null)
            {
                if (EnumerableStep.IsEnumerable(type))
                {
                    return tryFillEnumerableOfAllKnown(type);
                }

                return null;
            }

            return findStep(@default);
        }

        private BuildStep findStep(ServiceDescriptor descriptor)
        {
            if (descriptor?.ImplementationType != null)
            {
                var ctor = _graph.ChooseConstructor(descriptor.ImplementationType);
                if (ctor != null)
                {
                    return new ConstructorBuildStep(descriptor, ctor);
                }
            }

            return null;
        }

        private BuildStep tryFillEnumerableOfAllKnown(Type serviceType)
        {
            var elementType = EnumerableStep.DetermineElementType(serviceType);
            var all = _graph.FindAll(elementType);

            if (!all.All(x => _graph.CanResolve(x)))
            {
                return null;
            }

            var childSteps = all.Select(FindStep).ToArray();
            return new EnumerableStep(serviceType, childSteps);
        }
    }
}
