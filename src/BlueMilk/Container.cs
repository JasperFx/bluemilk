using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Baseline;
using BlueMilk.IoC;
using BlueMilk.IoC.Instances;
using BlueMilk.Scanning;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk
{
    public class Container : Scope, IServiceScopeFactory
    {
        public new static Container Empty()
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
        
        
        

        public override void Dispose()
        {

            
            base.Dispose();
            ServiceGraph.Dispose();
        }
        

        
        
    }

}