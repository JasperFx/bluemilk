using System;
using System.Collections.Generic;
using System.Linq;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Enumerables
{
    public class ListInstance<T> : Instance, IInstanceThatGeneratesResolver
    {
        private GeneratedType _resolverType;
        private Instance[] _elements;
        
        public ListInstance(Type serviceType) : base(serviceType, typeof(List<T>), ServiceLifetime.Transient)
        {
            Name = Variable.DefaultArgName(typeof(List<T>));
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            // This is goofy, but if the current service is the top level root of the resolver
            // being created here, make the dependencies all be Dependency mode
            var dependencyMode = isRoot && mode == BuildMode.Build ? BuildMode.Dependency : mode;
            
            var elements = _elements.Select(x => variables.Resolve(x, dependencyMode)).ToArray();
            
            return new ListAssignmentFrame<T>(this, elements).Variable;
        }

        protected override IResolver buildResolver(Scope rootScope)
        {
            return (IResolver) rootScope.QuickBuild(_resolverType.CompiledType);
        }

        protected override IEnumerable<Instance> createPlan(ServiceGraph services)
        {
            _elements = services.FindAll(typeof(T));
            return _elements;
        }

        public void GenerateResolver(GeneratedAssembly generatedAssembly)
        {
            // TODO -- lots of duplication in here
            var typeName = (ImplementationType.FullNameInCode() + "_" + Name).Replace('<', '_').Replace('>', '_').Replace(" ", "")
                .Replace(',', '_').Replace('.', '_').Replace("[", "").Replace("]", "");
            
            _resolverType = generatedAssembly.AddType(typeName, typeof(TransientResolver<>).MakeGenericType(ServiceType));

            var method = _resolverType.MethodFor("Build");

            var variable = CreateVariable(BuildMode.Build, new ResolverVariables(), true);

            method.ReturnVariable = variable;
            method.Frames.Add(variable.Creator);
        }

        public CreationStyle CreationStyle { get; } = CreationStyle.Generated;
    }
}