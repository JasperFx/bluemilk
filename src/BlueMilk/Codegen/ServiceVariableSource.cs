using System;
using BlueMilk.Codegen.ServiceLocation;
using BlueMilk.IoC.Planning;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.Codegen
{
    public class ServiceVariableSource : IVariableSource
    {
        private readonly IMethodVariables _method;
        private readonly ServiceGraph _services;

        public ServiceVariableSource(IMethodVariables method, ServiceGraph services)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _services = services;
        }

        public bool Matches(Type type)
        {
            // TODO -- Do we really want to do this this way?
            return true;
        }

        public Variable Create(Type type)
        {
            var @default = _services.FindDefault(type);
            BuildStepPlanner planner = null;
            BuildStep step = null;
            
            if (@default?.ImplementationType != null && @default.Lifetime != ServiceLifetime.Singleton)
            {
                planner = new BuildStepPlanner(_services, _method);
                step = planner.PlanConcreteBuild(@default);
            }
            
            


            return new ServiceCreationFrame(type, planner, step).Service;
        }
    }
}
