using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Planning;
using BlueMilk.IoC.Resolvers;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class ConstructorInstance<T> : ConstructorInstance
    {
        public ConstructorInstance(Type serviceType, ServiceLifetime lifetime) : base(serviceType, typeof(T), lifetime)
        {
        }
    }
     
    public class ConstructorInstance : Instance, IInstanceThatGeneratesResolver
    {
        public static readonly string NoPublicConstructors = "No public constructors";

        public static readonly string NoPublicConstructorCanBeFilled =
            "Cannot fill the dependencies of any of the public constructors";

        private Instance[] _arguments = new Instance[0];
        private GeneratedType _resolverType;

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

        public static ConstructorInstance<TConcrete> For<T, TConcrete>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TConcrete : T
        {
            return new ConstructorInstance<TConcrete>(typeof(T), lifetime);
        }

        public override IResolver BuildResolver(ResolverGraph resolvers, Scope rootScope)
        {
            if (_resolverType != null)
            {
                return (IResolver) rootScope.QuickBuild(_resolverType.CompiledType);
            }
            

            if (CreationStyle == CreationStyle.NoArg)
            {
                switch (Lifetime)
                {
                        case ServiceLifetime.Transient:
                            return typeof(NoArgTransientResolver<>).CloseAndBuildAs<IResolver>(ImplementationType);
                            
                        case ServiceLifetime.Scoped:
                            return typeof(NoArgScopedResolver<>).CloseAndBuildAs<IResolver>(ImplementationType);
                            
                        case ServiceLifetime.Singleton:
                            return typeof(NoArgSingletonResolver<>).CloseAndBuildAs<IResolver>(rootScope, ImplementationType);
                }

                return null;
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

                foreach (var argument in _arguments)
                {
                    argument.CreatePlan(services);
                }

                determineCreationStyleFromArguments();
            }


            return _arguments;
        }


        private void determineCreationStyleFromArguments()
        {
            if (_arguments.Any())
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
            else
            {
                CreationStyle = CreationStyle.NoArg;
            }

        }

        public static ConstructorInfo DetermineConstructor(NewServiceGraph services, Type implementationType,
            out string message)
        {
            message = null;

            var constructors = implementationType
                .GetConstructors() ?? new ConstructorInfo[0];


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
        
        

        public void GenerateResolver(GeneratedAssembly generatedAssembly)
        {
            var typeName = (ImplementationType.FullNameInCode() + "_" + Name).Replace('<', '_').Replace('>', '_').Replace(" ", "")
                .Replace(',', '_').Replace('.', '_');
            
            _resolverType = generatedAssembly.AddType(typeName, ResolverBaseType.MakeGenericType(ServiceType));

            var method = _resolverType.MethodFor("Build");

            var variable = CreateVariable(BuildMode.Build, new ResolverVariables(), true);

            method.ReturnVariable = variable;
            method.Frames.Add(variable.Creator);
        }
    }
}