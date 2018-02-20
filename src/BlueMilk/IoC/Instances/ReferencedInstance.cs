using System;
using System.Collections.Generic;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class ReferencedInstance : Instance
    {
        private readonly string _instanceKey;
        private Instance _inner;

        public ReferencedInstance(Type serviceType, string instanceKey) : base(serviceType, serviceType, ServiceLifetime.Transient)
        {
            _instanceKey = instanceKey;
        }

        public override object Resolve(Scope scope)
        {
            return _inner.Resolve(scope);
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            return _inner.CreateVariable(mode, variables, isRoot);
        }

        public override object QuickResolve(Scope scope)
        {
            return _inner.QuickResolve(scope);
        }

        public override bool RequiresServiceProvider => _inner.RequiresServiceProvider;
        
        public override Variable CreateInlineVariable(ResolverVariables variables)
        {
            return _inner.CreateInlineVariable(variables);
        }

        protected override IEnumerable<Instance> createPlan(ServiceGraph services)
        {
            _inner = services.FindInstance(ServiceType, _instanceKey);
            _inner.Parent = Parent;
            Lifetime = _inner.Lifetime;

            yield return _inner;
        }

        public override string GetBuildPlan()
        {
            return _inner.GetBuildPlan();
        }

        public override Instance CloseType(Type serviceType, Type[] templateTypes)
        {
            return _inner.CloseType(serviceType, templateTypes);
        }
    }
}