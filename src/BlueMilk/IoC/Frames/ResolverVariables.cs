using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Instances;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Frames
{
    public class ResolverVariables
    {
        private readonly IList<Variable> _variables = new List<Variable>();
        
        public ResolverVariables()
        {
        }

        public ResolverVariables(IEnumerable<Variable> existing)
        {
            _variables.AddRange(existing);
        }

        public Variable[] AllFor(Instance instance)
        {
            return _variables.Where(x => x.RefersTo(instance)).ToArray();
        }

        public Variable Resolve(Instance instance, BuildMode mode)
        {
            if (instance.Lifetime == ServiceLifetime.Transient) return instance.CreateVariable(mode, this, false);
            
            var variable = AllFor(instance).SingleOrDefault();
            if (variable == null)
            {
                variable = instance.CreateVariable(mode, this, false);
                _variables.Add(variable);
            }

            return variable;
        }

        public void Add(ServiceVariable variable)
        {
            // TODO -- have to do more on naming too
            var index = AllFor(variable.Instance).Length + 1;
            if (index > 1)
            {
                variable.OverrideName(variable.Usage + "_" + index);
            }
            
            _variables.Add(variable);
        }
    }
}