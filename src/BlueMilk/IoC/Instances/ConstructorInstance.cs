using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Planning;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public enum DisposeTracking
    {
        WithUsing,
        RegisterWithScope,
        None
    }


    public class ConstructorInstance : Instance
    {
        public static readonly string NoPublicConstructors = "No public constructors";

        public static readonly string NoPublicConstructorCanBeFilled =
            "Cannot fill the dependencies of any of the public constructors";

        private Instance[] _arguments = new Instance[0];

        public ConstructorInstance(Type serviceType, Type implementationType, ServiceLifetime lifetime) : base(
            serviceType, implementationType, lifetime)
        {
            Name = implementationType.NameInCode();
        }

        public CreationStyle CreationStyle { get; private set; }

        public ConstructorInfo Constructor { get; private set; }

        public Type ResolverBaseType { get; private set; }

        public static ConstructorInstance For<T>(ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            return For<T, T>(lifetime);
        }

        public static ConstructorInstance For<T, TConcrete>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TConcrete : T
        {
            return new ConstructorInstance(typeof(T), typeof(TConcrete), lifetime);
        }

        public override IResolver BuildResolver(ResolverGraph resolvers, Scope rootScope)
        {
            if (CreationStyle == CreationStyle.InlineSingleton) return null;

            if (CreationStyle == CreationStyle.NoArg)
            {
                var resolverType = Lifetime == ServiceLifetime.Scoped
                    ? typeof(NoArgScopedResolver<>)
                    : typeof(NoArgTransientResolver<>);

                return resolverType.CloseAndBuildAs<IResolver>(ImplementationType);
            }


            return null;
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            if (Lifetime == ServiceLifetime.Singleton)
                return mode == BuildMode.Build
                    ? generateVariableForBuilding(variables, mode, isRoot)
                    : new InjectedServiceField(this);

            


            if (Lifetime == ServiceLifetime.Scoped && mode == BuildMode.Dependency)
            {
                return new GetInstanceFrame(this).Variable;
            }
            
            
            
            return generateVariableForBuilding(variables, mode, isRoot);
        }

        private DisposeTracking determineDisposalTracking(BuildMode mode)
        {
            if (!ImplementationType.CanBeCastTo<IDisposable>()) return DisposeTracking.None;

            switch (mode)
            {
                case BuildMode.Inline:
                    return DisposeTracking.WithUsing;


                case BuildMode.Dependency:
                    return DisposeTracking.RegisterWithScope;


                case BuildMode.Build:
                    return DisposeTracking.None;
            }

            return DisposeTracking.None;
        }

        private Variable generateVariableForBuilding(ResolverVariables variables, BuildMode mode, bool isRoot)
        {
            var disposalTracking = determineDisposalTracking(mode);

            // This is goofy, but if the current service is the top level root of the resolver
            // being created here, make the dependencies all be Dependency mode
            var dependencyMode = isRoot && mode == BuildMode.Build ? BuildMode.Dependency : mode;

            var ctorParameters = _arguments.Select(arg => variables.Resolve(arg, dependencyMode)).ToArray();
            
            return new NewConstructorFrame(this, disposalTracking, ctorParameters).Variable;
        }

        protected override IEnumerable<Instance> createPlan(NewServiceGraph services)
        {
            Constructor = DetermineConstructor(services, ImplementationType, out var message);

            if (message.IsNotEmpty()) ErrorMessages.Add(message);


            if (Constructor != null)
            {
                // TODO -- this will need to get smarter when we have inline dependencies and named stuff
                _arguments = Constructor.GetParameters().Select(x => services.FindDefault(x.ParameterType)).ToArray();

                foreach (var argument in _arguments) argument.CreatePlan(services);

                if (_arguments.Any())
                    determineCreationStyleFromArguments();
                else
                    determineNoArgCreationStyle();
            }


            return _arguments;
        }

        private void determineNoArgCreationStyle()
        {
            CreationStyle = Lifetime == ServiceLifetime.Singleton
                ? CreationStyle.InlineSingleton
                : CreationStyle.NoArg;
        }

        private void determineCreationStyleFromArguments()
        {
            if (Lifetime == ServiceLifetime.Singleton && !_arguments.Any(x => x.RequiresServiceProvider))
            {
                CreationStyle = CreationStyle.InlineSingleton;
            }
            else
            {
                CreationStyle = CreationStyle.Generated;

                switch (Lifetime)
                {
                    case ServiceLifetime.Scoped:
                        ResolverBaseType = typeof(ScopedResolver<>);
                        break;

                    case ServiceLifetime.Singleton:
                        ResolverBaseType = typeof(SingletonResolver<>);
                        break;

                    case ServiceLifetime.Transient:
                        ResolverBaseType = typeof(TransientResolver<>);
                        break;
                }
            }
        }

        public static ConstructorInfo DetermineConstructor(NewServiceGraph services, Type implementationType,
            out string message)
        {
            message = null;

            var constructors = implementationType
                .GetConstructors();


            if (constructors.Any())
            {
                var ctor = constructors
                    .OrderByDescending(x => x.GetParameters().Length)
                    .FirstOrDefault(services.CouldBuild);

                if (ctor == null) message = NoPublicConstructorCanBeFilled;

                return ctor;
            }

            message = NoPublicConstructors;

            return null;
        }
    }
}