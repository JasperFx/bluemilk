using System;
using System.Collections.Generic;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.Testing.IoC.Acceptance
{
    public class custom_instance_usage
    {
        
    }

    public class SessionInstance : Instance
    {
        private Instance _store;

        public SessionInstance(Type serviceType, Type implementationType, ServiceLifetime lifetime) : base(serviceType, implementationType, lifetime)
        {
        }

        protected override IEnumerable<Instance> createPlan(ServiceGraph services)
        {
            _store = services.FindDefault(typeof(IStore));
            yield return _store;
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            throw new NotImplementedException();
        }

        protected override IResolver buildResolver(Scope rootScope)
        {
            throw new NotImplementedException();
        }
    }
    
    public interface ISession{}
    public class Session : ISession {}

    public interface IStore
    {
        ISession Open();
    }

    public class Store : IStore
    {
        public ISession Open()
        {
            return new Session();
        }
    }
}