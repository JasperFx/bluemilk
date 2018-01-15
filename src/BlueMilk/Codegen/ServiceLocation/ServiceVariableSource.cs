using System;
using BlueMilk.Codegen.Variables;

namespace BlueMilk.Codegen.ServiceLocation
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
            return _services.CanResolve(type);
        }

        public Variable Create(Type type)
        {
            return new ServiceCreationFrame(type).Service;
        }
    }
}
