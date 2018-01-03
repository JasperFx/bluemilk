using System;
using BlueMilk.Codegen;

namespace BlueMilk.IoC.Instances
{
    public class ObjectInstance : Instance
    {
        public static ObjectInstance For<T>(T @object)
        {
            return new ObjectInstance(typeof(T), @object);
        }
        
        public ObjectInstance(Type serviceType, object instance) : base(serviceType, instance)
        {
            Name = instance?.GetType().NameInCode() ?? serviceType.NameInCode();
        }
        
        
    }
}