using System;
using System.Collections.Generic;
using Baseline;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC
{
    public class Scope : IServiceScope
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public IList<IDisposable> Disposables { get; } = new List<IDisposable>();

        public void RegisterDisposable(IDisposable disposable)
        {
            Disposables.Add(disposable);
        }

        // TODO -- this will need to register the service by name later
        public void Register(Type serviceType, object service)
        {
            _services.Add(serviceType, service);
        }

        public bool TryFind<T>(out T service)
        {
            if (_services.ContainsKey(typeof(T)))
            {
                service = (T) _services[typeof(T)];
                return true;
            }

            service = default(T);
            return false;
        }

        public void Dispose()
        {
            foreach (var disposable in Disposables)
            {
                disposable.SafeDispose();
            }
        }

        // TODO -- not wild about this having to be externally set
        // Reevaluate
        public IServiceProvider ServiceProvider { get; set; }
    }
}