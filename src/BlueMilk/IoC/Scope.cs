using System;
using System.Collections.Generic;
using Baseline;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC
{
    public class Scope : IServiceScope, IDisposable
    {
        private readonly IList<IDisposable> _disposables = new List<IDisposable>();
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        
        public void RegisterDisposable(IDisposable disposable)
        {
            _disposables.Add(disposable);
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
            foreach (var disposable in _disposables)
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