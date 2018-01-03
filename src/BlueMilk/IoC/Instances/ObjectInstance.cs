using System;
using System.Reflection;
using BlueMilk.Codegen;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class ObjectInstance : Instance
    {
        public static ObjectInstance For<T>(T @object)
        {
            return new ObjectInstance(typeof(T), @object);
        }
        
        public ObjectInstance(Type serviceType, object instance) : base(serviceType, ServiceLifetime.Singleton)
        {
            Name = instance?.GetType().NameInCode() ?? serviceType.NameInCode();
        }

        public override IResolver BuildResolver(Assembly dynamicAssembly, ResolverGraph resolvers, Scope rootScope)
        {
            throw new NotImplementedException();
        }
    }
}