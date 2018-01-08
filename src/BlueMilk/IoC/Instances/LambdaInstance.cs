using System;
using System.Reflection;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Planning;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class LambdaInstance : Instance
    {
        public Func<IServiceProvider, object> Factory { get; }

        public LambdaInstance(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime) :
            base(serviceType, serviceType, lifetime)
        {
            Factory = factory;
            Name = serviceType.NameInCode();
        }

        public static LambdaInstance For<T>(Func<IServiceProvider, T> factory,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            return new LambdaInstance(typeof(T), s => factory(s), lifetime);
        }

        public override bool RequiresServiceProvider { get; } = true;

        public override ServiceVariable CreateVariable(BuildMode mode)
        {
            return new GetInstanceFrame(this).Variable;
        }

        public override IResolver BuildResolver(ResolverGraph resolvers, Scope rootScope)
        {
            switch (Lifetime)
            {
                case ServiceLifetime.Transient:
                    return typeof(TransientLambdaResolver<>).CloseAndBuildAs<IResolver>(Factory, ServiceType);

                case ServiceLifetime.Scoped:
                    return typeof(ScopedLambdaResolver<>).CloseAndBuildAs<IResolver>(Factory, ServiceType);

                case ServiceLifetime.Singleton:
                    return typeof(SingletonLambdaResolver<>).CloseAndBuildAs<IResolver>(Factory, rootScope, ServiceType);
            }
            
            throw new ArgumentOutOfRangeException(nameof(Lifetime));
        }


    }
}