using System;
using System.Linq;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public interface IInstanceThatGeneratesResolver
    {
        void GenerateResolver(GeneratedAssembly generatedAssembly);
    }

    public abstract class GeneratedInstance : Instance, IInstanceThatGeneratesResolver
    {
        private GeneratedType _resolverType;
        
        protected GeneratedInstance(Type serviceType, Type implementationType, ServiceLifetime lifetime) : base(serviceType, implementationType, lifetime)
        {
        }

        public abstract Frame CreateBuildFrame();
        
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
        
        public sealed override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
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

        protected abstract Variable generateVariableForBuilding(ResolverVariables variables, BuildMode mode,
            bool isRoot);
        
        
        protected override IResolver buildResolver(Scope rootScope)
        {
            if (_resolverType != null)
            {
                return (IResolver) rootScope.QuickBuild(_resolverType.CompiledType);
            }
            
            return null;
        }
        
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
    }
}