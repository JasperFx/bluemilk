using System;
using System.ComponentModel;
using System.Linq;
using Baseline;
using BlueMilk.IoC;
using BlueMilk.IoC.Instances;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk
{
    public class Container : Scope, IContainer, IServiceScopeFactory, IDisposable
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

        public T QuickBuild<T>()
        {
            if (!typeof(T).IsConcrete()) throw new InvalidOperationException("Type must be concrete");

            var ctor = ConstructorInstance.DetermineConstructor(ServiceGraph, typeof(T), out var message);
            if (ctor == null) throw new InvalidOperationException(message);

            var dependencies = ctor.GetParameters().Select(x => GetInstance(x.ParameterType)).ToArray();

            return (T) Activator.CreateInstance(typeof(T), dependencies);
        }


        public override void Dispose()
        {
            ServiceGraph.Dispose();
            base.Dispose();
        }
    }

}