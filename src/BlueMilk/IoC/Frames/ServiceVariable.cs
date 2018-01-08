using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Instances;

namespace BlueMilk.IoC.Frames
{
    public class ServiceVariable : Variable
    {
        public ServiceVariable(Instance instance, Frame creator) : base(instance.ImplementationType, instance.Name, creator)
        {
            Instance = instance;
        }
        
        public Instance Instance { get; }
    }
}