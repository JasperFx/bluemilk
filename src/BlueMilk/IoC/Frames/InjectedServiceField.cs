using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Instances;

namespace BlueMilk.IoC.Frames
{
    public class InjectedServiceField : InjectedField, IServiceVariable
    {
        public InjectedServiceField(Instance instance) : base(instance.ServiceType)
        {
            Instance = instance;
        }

        public Instance Instance { get; }
    }
}