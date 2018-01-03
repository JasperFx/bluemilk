using System;
using System.Collections.Generic;
using Baseline;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC
{
    public class Scope : IServiceScope, IDisposable
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public IList<IDisposable> Disposables { get; } = new List<IDisposable>();

        public void RegisterDisposable(IDisposable disposable)
        {
            Disposables.Add(disposable);
        }

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

        public IServiceProvider ServiceProvider
        {
            get { throw new NotImplementedException(); }
        }
    }
}