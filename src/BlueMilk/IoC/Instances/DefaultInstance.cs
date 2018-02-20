using System;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class DefaultInstance : Instance
    {
        public DefaultInstance(Type serviceType, Type implementationType, ServiceLifetime lifetime) : base(serviceType, implementationType, lifetime)
        {
        }

        public DefaultInstance(Type serviceType) : base(serviceType, serviceType, ServiceLifetime.Transient)
        {
        }

        public override object Resolve(Scope scope)
        {
            throw new NotSupportedException();
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            throw new NotSupportedException();
        }
    }
}