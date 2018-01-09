using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.IoC.Instances;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC
{
    public class Scope : IContainer, IServiceScope, IServiceProvider
    {
        public static Scope Empty()
        {
            return new Scope(new ServiceRegistry());
        }
        
        private readonly Dictionary<Type, object> _servicesByType = new Dictionary<Type, object>();

        public Scope(IServiceCollection services)
        {
            ServiceGraph = new NewServiceGraph(services, this);
            
            ServiceGraph.Initialize();
            Resolvers = ServiceGraph.Resolvers;
        }

        public Scope(NewServiceGraph serviceGraph)
        {
            ServiceGraph = serviceGraph;
            Resolvers = ServiceGraph.Resolvers;
        }

        public ResolverGraph Resolvers { get; }

        public NewServiceGraph ServiceGraph { get; }


        public IList<IDisposable> Disposables { get; } = new List<IDisposable>();

        public void RegisterDisposable(IDisposable disposable)
        {
            Disposables.Add(disposable);
        }

        // TODO -- this will need to register the service by name later
        public void Register(Type serviceType, object service)
        {
            _servicesByType.Add(serviceType, service);
        }

        public bool TryFind<T>(out T service)
        {
            if (_servicesByType.ContainsKey(typeof(T)))
            {
                service = (T) _servicesByType[typeof(T)];
                return true;
            }

            service = default(T);
            return false;
        }

        public virtual void Dispose()
        {
            foreach (var disposable in Disposables)
            {
                disposable.SafeDispose();
            }
        }

        public IServiceProvider ServiceProvider => this;
        
        // TODO -- really the same thing as TryGetInstance in StructureMap
        public object GetService(Type serviceType)
        {
            return GetInstance(serviceType);
        }
        
        public T GetInstance<T>()
        {
            // TODO -- sad path, not found
            // TODO -- validate object disposed
            return (T) GetInstance(typeof(T));
        }

        public T GetInstance<T>(string name)
        {
            return (T) GetInstance(typeof(T), name);
        }

        public object GetInstance(Type serviceType)
        {
            var resolver = Resolvers.ByType[serviceType];
            return resolver.Resolve(this);
        }

        public object GetInstance(Type serviceType, string name)
        {
            // TODO -- sad path, not found
            // TODO -- validate object disposed
            var resolver = Resolvers.ByTypeAndName[serviceType]?[name];
            return resolver.Resolve(this);
        }

        public T QuickBuild<T>()
        {
            return (T) QuickBuild(typeof(T));

        }

        public object QuickBuild(Type objectType)
        {
            if (!objectType.IsConcrete()) throw new InvalidOperationException("Type must be concrete");

            var ctor = ConstructorInstance.DetermineConstructor(ServiceGraph, objectType, out var message);
            if (ctor == null) throw new InvalidOperationException(message);

            var dependencies = ctor.GetParameters().Select(x => GetInstance(x.ParameterType)).ToArray();

            return Activator.CreateInstance(objectType, dependencies);
        }
    }
}