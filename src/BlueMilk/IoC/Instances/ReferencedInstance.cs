using System;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class ReferencedInstance : Instance
    {
        public ReferencedInstance(Type serviceType, Type implementationType, ServiceLifetime lifetime) : base(serviceType, implementationType, lifetime)
        {
        }

        public ReferencedInstance(Type serviceType, string instanceKey) : base(serviceType, serviceType, ServiceLifetime.Transient)
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