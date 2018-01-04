using System;
using System.Linq;
using System.Reflection;

namespace BlueMilk.Codegen
{
    public static class ServiceProviderExtensions
    {
        public static object Build(this IServiceProvider services, Type type)
        {
            var ctor = type.GetTypeInfo().GetConstructors().Single();
            var inputs = ctor.GetParameters().Select(x => services.GetService(x.ParameterType)).ToArray();
            return Activator.CreateInstance(type, inputs);
        }
    }
}