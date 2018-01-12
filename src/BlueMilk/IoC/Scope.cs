using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.Codegen;
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
        


        public Scope(IServiceCollection services)
        {
            ServiceGraph = new ServiceGraph(services, this);
            ServiceGraph.Initialize();
        }

        public Scope(ServiceGraph serviceGraph)
        {
            ServiceGraph = serviceGraph;
        }

        public DisposalLock DisposalLock { get; set; } = DisposalLock.Unlocked;

        public IModel Model => ServiceGraph;

        internal ServiceGraph ServiceGraph { get; }


        // TODO -- hide this from the public class?
        public IList<IDisposable> Disposables { get; } = new List<IDisposable>();

        public void RegisterDisposable(IDisposable disposable)
        {
            Disposables.Add(disposable);
        }
        
        internal readonly Dictionary<int, object> Services = new Dictionary<int, object>();


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
            var resolver = ServiceGraph.FindResolver(serviceType);
            
            // TODO -- validate the existence of the resolver first
            if (resolver == null)
            {
                throw new BlueMilkMissingRegistrationException(serviceType);
            }
            
            return resolver.Resolve(this);
        }

        public object GetInstance(Type serviceType, string name)
        {
            // TODO -- sad path, not found
            // TODO -- validate object disposed
            var resolver = ServiceGraph.FindResolver(serviceType, name);
            if (resolver == null)
            {
                throw new BlueMilkMissingRegistrationException(serviceType, name);
            }
            
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

        public IContainer GetNestedContainer()
        {
            return new Scope(ServiceGraph);
        }

        public IReadOnlyList<T> GetAllInstances<T>()
        {
            return ServiceGraph.FindAll(typeof(T)).Select(x => x.Resolver.Resolve(this)).OfType<T>().ToList();
        }

        public IEnumerable GetAllInstances(Type serviceType)
        {
            return ServiceGraph.FindAll(serviceType).Select(x => x.Resolver.Resolve(this)).ToArray();
        }
    }


    public class BlueMilkException : Exception
    {
        public BlueMilkException(string message) : base(message)
        {
        }

        public BlueMilkException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class BlueMilkMissingRegistrationException : BlueMilkException
    {
        public BlueMilkMissingRegistrationException(Type serviceType, string name) : base($"Unknown service registration '{name}' of {serviceType.FullNameInCode()}")
        {
        }

        public BlueMilkMissingRegistrationException(Type serviceType) : base($"No service registrations exist or can be derived for {serviceType.FullNameInCode()}")
        {
        }
    }
    
    
}