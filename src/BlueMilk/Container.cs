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
            throw new NotImplementedException();
        }


        public override void Dispose()
        {
            ServiceGraph.Dispose();
            base.Dispose();
        }
    }

}