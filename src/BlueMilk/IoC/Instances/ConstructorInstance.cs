using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Resolvers;
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

        private CtorArg[] _arguments = new CtorArg[0];
        private GeneratedType _resolverType;

        public ConstructorInstance(Type serviceType, Type implementationType, ServiceLifetime lifetime) : base(
            serviceType, implementationType, lifetime)
        {
            Name = Variable.DefaultArgName(implementationType);
        }

        public ConstructorInfo Constructor { get; private set; }

        public Type ResolverBaseType
        {
            get
            {
                switch (Lifetime)
                {
                    case ServiceLifetime.Scoped:
                        return typeof(ScopedResolver<>);

                    case ServiceLifetime.Singleton:
                        return typeof(SingletonResolver<>);

                    case ServiceLifetime.Transient:
                        return typeof(TransientResolver<>);
                }

                return null;
            }
        }

        public static ConstructorInstance For<T>(ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            return For<T, T>(lifetime);
        }

        public static ConstructorInstance<TConcrete> For<T, TConcrete>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TConcrete : T
        {
            return new ConstructorInstance<TConcrete>(typeof(T), lifetime);
        }

        public override Instance CloseType(Type serviceType, Type[] templateTypes)
        {
            if (!ImplementationType.IsOpenGeneric())
                return null;

            Type closedType;
            try
            {
                closedType = ImplementationType.MakeGenericType(templateTypes);
            }
            catch
            {
                return null;
            }

            var closedInstance = new ConstructorInstance(serviceType, closedType, Lifetime);

            // TODO' -- later!
            //Dependencies.Each(arg => closedInstance.Dependencies.Add(arg.CloseType(types)));
            

            return closedInstance;
        }

        protected override IResolver buildResolver(Scope rootScope)
        {
            if (_resolverType != null)
            {
                return (IResolver) rootScope.QuickBuild(_resolverType.CompiledType);
            }
            
            return null;
        }
        
        public Frame CreateBuildFrame()
        {
            var variables = new ResolverVariables();
            var ctorParameters = _arguments.Select(arg => arg.Resolve(variables, BuildMode.Dependency)).ToArray();
            
            return new ConstructorFrame(this, DisposeTracking.None, ctorParameters)
            {
                ReturnCreated = true
            };
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

            var ctorParameters = _arguments.Select(arg => arg.Resolve(variables, dependencyMode)).ToArray();
            
            var frame = new ConstructorFrame(this, disposalTracking, ctorParameters).Variable;


            return frame;
        }

        protected override IEnumerable<Instance> createPlan(ServiceGraph services)
        {
            Constructor = DetermineConstructor(services, ImplementationType, out var message);

            if (message.IsNotEmpty()) ErrorMessages.Add(message);


            if (Constructor != null)
            {
                // TODO -- this will need to get smarter when we have inline dependencies and named stuff
                
                
                _arguments = Constructor.GetParameters().Select(x => new CtorArg(x, services.FindDefault(x.ParameterType))).Where(x => x.Instance != null).ToArray();


                foreach (var argument in _arguments)
                {
                    argument.Instance.CreatePlan(services);
                }
            }


            return _arguments.Select(x => x.Instance);
        }

        public class CtorArg
        {
            public ParameterInfo Parameter { get; }
            public Instance Instance { get; }

            public CtorArg(ParameterInfo parameter, Instance instance)
            {
                Parameter = parameter;
                Instance = instance;
            }

            public Variable Resolve(ResolverVariables variables, BuildMode mode)
            {
                var inner = variables.Resolve(Instance, mode);
                if (Parameter.IsOptional)
                {
                    var wrapped = new Variable(inner.VariableType, $"{Parameter.Name}: {inner.Usage}");
                    wrapped.Dependencies.Add(inner);

                    return wrapped;
                }
                else
                {
                    return inner;
                }
            }
        }



        public override string ToString()
        {
            if (Constructor != null)
            {
                return $"new {ImplementationType.FullNameInCode()}({Constructor.GetParameters().Select(x => x.ParameterType.NameInCode()).Join(", ")})";
            }

            return $"new {ImplementationType.FullNameInCode()}()";
        }

        public static ConstructorInfo DetermineConstructor(ServiceGraph services, Type implementationType,
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
            if (ResolverBaseType == null || ErrorMessages.Any()) return;
            
            var typeName = (ServiceType.FullNameInCode() + "_" + Name).Replace('<', '_').Replace('>', '_').Replace(" ", "")
                .Replace(',', '_').Replace('.', '_');
            
            _resolverType = generatedAssembly.AddType(typeName, ResolverBaseType.MakeGenericType(ServiceType));

            var method = _resolverType.MethodFor("Build");

            var frame = CreateBuildFrame();

            method.Frames.Add(frame);
            
            
        }


    }
}