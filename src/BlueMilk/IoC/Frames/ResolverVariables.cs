using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Planning;

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

        public ServiceVariable[] AllFor(Instance instance)
        {
            return _variables.OfType<ServiceVariable>().Where(x => x.Instance == instance).ToArray();
        }

        public ServiceVariable For(Instance instance, BuildMode mode)
        {
            var variable = AllFor(instance).SingleOrDefault();
            if (variable == null)
            {
                variable = instance.CreateVariable(mode, this);
                _variables.Add(variable);
            }

            return variable;
        }

        public void Add(ServiceVariable variable)
        {
            var index = AllFor(variable.Instance).Length + 1;
            if (index > 1)
            {
                variable.OverrideName(variable.Usage + "_" + index);
            }
            
            _variables.Add(variable);
        }
    }
}