using System;
using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Instances;

namespace BlueMilk.IoC.Frames
{
    public class ServiceVariable : Variable, IServiceVariable
    {
        public ServiceVariable(Instance instance, Frame creator) : base(instance.ImplementationType, instance.Name.Replace(".", "_"), creator)
        {
            Instance = instance;
        }
        
        public Instance Instance { get; }
    }
}