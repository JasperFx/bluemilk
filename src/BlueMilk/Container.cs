using System;
using System.ComponentModel;
using System.Linq;
using Baseline;
using BlueMilk.IoC;
using BlueMilk.IoC.Instances;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk
{
    public class Container : Scope, IServiceScopeFactory
    {
        public static Container Empty()
        {
            return For(_ => { });
        }

        public static Container For<T>() where T : ServiceRegistry, new()
        {
            return new Container(new T());
        }
        
        public static Container For(Action<ServiceRegistry> configuration)
        {
            var registry = new ServiceRegistry();
            configuration(registry);
            
            return new Container(registry);
        }
        
        
        public Container(IServiceCollection services) : base(services)
        {

        }

        public Container(Action<ServiceRegistry> configuration) : this(ServiceRegistry.For(configuration))
        {
            
        }
        

        public IServiceScope CreateScope()
        {
            return new Scope(ServiceGraph);
        }
        
        
        private bool _disposedLatch;

        public override void Dispose()
        {
            if (DisposalLock == DisposalLock.Ignore) return;

            if (DisposalLock == DisposalLock.ThrowOnDispose) throw new InvalidOperationException("This Container has DisposalLock = DisposalLock.ThrowOnDispose and cannot be disposed until the lock is cleared");

            if (_disposedLatch) return;
            _disposedLatch = true;

            
            ServiceGraph.Dispose();
            base.Dispose();
        }
    }

}