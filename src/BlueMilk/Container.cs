using System;
using System.ComponentModel;
using BlueMilk.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk
{

    public interface IContainer
    {
        /// <summary>
        /// Creates or finds the default instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type which instance is to be created or found.</typeparam>
        /// <returns>The default instance of <typeparamref name="T"/>.</returns>
        T GetInstance<T>();

        /// <summary>
        /// Creates or finds the named instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type which instance is to be created or found.</typeparam>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The named instance of <typeparamref name="T"/>.</returns>
        T GetInstance<T>(string name);

        /// <summary>
        /// Creates or finds the default instance of <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type which instance is to be created or found.</param>
        /// <returns>The default instance of <paramref name="serviceType"/>.</returns>
        object GetInstance(Type serviceType);

        /// <summary>
        /// Creates or finds the named instance of <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type which instance is to be created or found.</param>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The named instance of <paramref name="serviceType"/>.</returns>
        object GetInstance(Type serviceType, string name);

    }

    public class Container : IContainer, IServiceProvider, IServiceScopeFactory, IDisposable
    {
        private readonly NewServiceGraph _services;
        private readonly ResolverGraph _resolvers;
        private readonly Scope _scope;

        public static Container For(Action<ServiceRegistry> configuration)
        {
            var registry = new ServiceRegistry();
            configuration(registry);
            
            return new Container(registry);
        }
        
        
        public Container(IServiceCollection services)
        {
            _scope = new Scope();
            _services = new NewServiceGraph(services, _scope);
            _resolvers = _services.Resolvers;

            // Yes Dorothy, this is some circular reference action going on here.
            // We'll get it sorted out later

            _scope.ServiceProvider = this;
        }

        public Container(Action<ServiceRegistry> configuration) : this(ServiceRegistry.For(configuration))
        {
            
        }
        
        
        // TODO -- really the same thing as TryGetInstance in StructureMap
        public object GetService(Type serviceType)
        {
            return GetInstance(serviceType);
        }

        public IServiceScope CreateScope()
        {
            throw new NotImplementedException();
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
            var resolver = _resolvers.ByType[serviceType];
            return resolver.Resolve(_scope);
        }

        public object GetInstance(Type serviceType, string name)
        {
            // TODO -- sad path, not found
            // TODO -- validate object disposed
            var resolver = _resolvers.ByTypeAndName[serviceType]?[name];
            return resolver.Resolve(_scope);
        }

        public void Dispose()
        {
            _scope.Dispose();
            _services.Dispose();
        }
    }

}