using System;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class NullInstance : Instance, IResolver
    {
        public NullInstance(Type serviceType) : base(serviceType, serviceType, ServiceLifetime.Transient)
        {
            Hash = GetHashCode();
        }

        public override IResolver ToResolver(Scope topScope)
        {
            return this;
        }

        public override object Resolve(Scope scope)
        {
            return null;
        }

        public int Hash { get; set; }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            return new Variable(ServiceType, "null");
        }
    }
}