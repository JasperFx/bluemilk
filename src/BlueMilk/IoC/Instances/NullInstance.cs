using System;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class NullInstance : Instance
    {
        public NullInstance(Type serviceType) : base(serviceType, serviceType, ServiceLifetime.Transient)
        {
            Hash = GetHashCode();
        }

        public override Func<Scope, object> ToResolver(Scope topScope)
        {
            return s => null;
        }

        public override object Resolve(Scope scope)
        {
            return null;
        }


        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            return new Variable(ServiceType, "null");
        }
    }
}