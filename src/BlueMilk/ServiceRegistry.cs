using System;
using System.Collections.Generic;
using BlueMilk.IoC.Instances;
using BlueMilk.Scanning.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk
{
    public static class InstanceExtensions
    {
        public static T Named<T>(this T instance, string name) where T : Instance
        {
            instance.Name = name;
            return instance;
        }

        public static T Scoped<T>(this T instance) where T : Instance
        {
            instance.Lifetime = ServiceLifetime.Scoped;
            return instance;
        }
        
        public static T Singleton<T>(this T instance) where T : Instance
        {
            instance.Lifetime = ServiceLifetime.Singleton;
            return instance;
        }
        
        public static T Transient<T>(this T instance) where T : Instance
        {
            instance.Lifetime = ServiceLifetime.Transient;
            return instance;
        }
    }
    
    public class ServiceRegistry : List<ServiceDescriptor>, IServiceCollection
    {
        public static ServiceRegistry For(Action<ServiceRegistry> configuration)
        {
            var registry = new ServiceRegistry();
            configuration(registry);

            return registry;
        }
        
        public ServiceRegistry()
        {
        }
        
        

        public DescriptorExpression<T> For<T>() where T : class
        {
            return new DescriptorExpression<T>(this, ServiceLifetime.Transient);
        }

        public class DescriptorExpression<T> where T : class
        {
            private readonly ServiceRegistry _parent;
            private readonly ServiceLifetime _lifetime;

            public DescriptorExpression(ServiceRegistry parent, ServiceLifetime lifetime)
            {
                _parent = parent;
                _lifetime = lifetime;
            }

            public ConstructorInstance<TConcrete> Use<TConcrete>() where TConcrete : class, T
            {
                var instance = ConstructorInstance.For<T, TConcrete>();
                instance.Lifetime = _lifetime;

                _parent.Add(instance);

                return instance;
            }

            /// <summary>
            /// Fills in a default type implementation for a service type if there are no prior
            /// registrations
            /// </summary>
            /// <typeparam name="TConcrete"></typeparam>
            /// <exception cref="NotImplementedException"></exception>
            public void UseIfNone<TConcrete>() where TConcrete : class, T
            {
                if (_parent.FindDefault<T>() == null)
                {
                    Use<TConcrete>();
                }
            }

            /// <summary>
            /// Delegates to Use<TConcrete>(), polyfill for StructureMap syntax
            /// </summary>
            /// <typeparam name="TConcrete"></typeparam>
            /// <exception cref="NotImplementedException"></exception>
            public ConstructorInstance<TConcrete> Add<TConcrete>() where TConcrete : class, T
            {
                return Use<TConcrete>();
            }

            public ObjectInstance Use(T service)
            {
                var instance = new ObjectInstance(typeof(T), service);
                _parent.Add(instance);

                return instance;
            }

            public ObjectInstance Add(T instance)
            {
                return Use(instance);
            }

            public DescriptorExpression<T> Scoped()
            {
                
                return this;
            }
        }

        public DescriptorExpression<T> ForSingletonOf<T>() where T : class
        {
            return new DescriptorExpression<T>(this, ServiceLifetime.Singleton);
        }

        public void Scan(Action<IAssemblyScanner> scan)
        {
            var finder = new AssemblyScanner();
            scan(finder);

            finder.Start();

            var descriptor = ServiceDescriptor.Singleton(finder);
            Add(descriptor);
        }


        public void IncludeRegistry<T>() where T : ServiceRegistry, new()
        {
            this.AddRange(new T());
        }
    }
}
