using System;
using System.Reflection;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class LambdaInstance : Instance
    {
        public Func<IServiceProvider, object> Factory { get; }

        public LambdaInstance(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime) :
            base(serviceType, lifetime)
        {
            Factory = factory;
            Name = serviceType.NameInCode();
        }

        public static LambdaInstance For<T>(Func<IServiceProvider, T> factory,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            return new LambdaInstance(typeof(T), s => factory(s), lifetime);
        }

        public override void RegisterResolver(Assembly dynamicAssembly, ResolverGraph resolvers, Scope rootScope)
        {
            throw new NotImplementedException();
//            switch (Lifetime)
//            {
//                case ServiceLifetime.Transient:
//                    return typeof(TransientLambdaResolver<>).Clo;
//
//                case ServiceLifetime.Scoped:
//                    return typeof(ScopedLambdaResolver<>);
//
//                case ServiceLifetime.Singleton:
//                    return typeof(SingletonLambdaResolver<>);
//            }
//            
//            var resolver = determineResolverType().CloseAndBuildAs<IResolver>(this, ServiceType);
//            resolvers.ByType[ServiceType] = resolver;
        }

    }
}