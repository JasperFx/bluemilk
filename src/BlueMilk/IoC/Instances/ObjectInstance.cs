using System;
using System.Reflection;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Planning;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class ObjectInstance : Instance, IResolver, IDisposable
    {
        public static ObjectInstance For<T>(T @object)
        {
            return new ObjectInstance(typeof(T), @object);
        }
        
        public ObjectInstance(Type serviceType, object service) : base(serviceType, service?.GetType() ?? serviceType, ServiceLifetime.Singleton)
        {
            Name = service?.GetType().NameInCode() ?? serviceType.NameInCode();
            Service = service;
        }

        public object Service { get; }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            return new InjectedServiceField(this);
        }

        public override IResolver BuildResolver(ResolverGraph resolvers, Scope rootScope)
        {
            return this;
        }

        object IResolver.Resolve(Scope scope)
        {
            return Service;
        }

        public void Dispose()
        {
            (Service as IDisposable)?.Dispose();
        }
    }
}