using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Enumerables
{
    public class EnumerablePolicy : IFamilyPolicy
    {
        public static bool IsEnumerable(Type type)
        {
            if (type.IsArray) return true;

            return type.IsGenericType && _enumerableTypes.Contains(type.GetGenericTypeDefinition());
        }

        public static Type DetermineElementType(Type serviceType)
        {
            if (serviceType.IsArray)
            {
                return serviceType.GetElementType();
            }

            return serviceType.GetGenericArguments().First();
        }
        
        private static readonly List<Type> _enumerableTypes = new List<Type>
        {
            typeof (IEnumerable<>),
            typeof (IList<>),
            typeof (IReadOnlyList<>),
            typeof (List<>)
        };
        
        public ServiceFamily Build(Type type, ServiceGraph serviceGraph)
        {
            if (type.IsArray)
            {
                var instanceType = typeof(ArrayInstance<>).MakeGenericType(type.GetElementType());
                var instance = Activator.CreateInstance(instanceType, type).As<Instance>();
                return new ServiceFamily(type, instance);
            }

            if (type.IsGenericType && _enumerableTypes.Contains(type.GetGenericTypeDefinition()))
            {
                var elementType = type.GetGenericArguments().First();
                
                var instanceType = typeof(ListInstance<>).MakeGenericType(elementType);
                var instance = Activator.CreateInstance(instanceType, type).As<Instance>();
                return new ServiceFamily(type, instance);
            }

            return null;
        }
    }

    public class ArrayInstance<T> : Instance, IInstanceThatGeneratesResolver
    {
        private GeneratedType _resolverType;
        private Instance[] _elements;

        public ArrayInstance(Type serviceType) : base(serviceType, typeof(T[]), ServiceLifetime.Transient)
        {
            Name = Variable.DefaultArgName<T[]>();
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            // This is goofy, but if the current service is the top level root of the resolver
            // being created here, make the dependencies all be Dependency mode
            var dependencyMode = isRoot && mode == BuildMode.Build ? BuildMode.Dependency : mode;
            
            var elements = _elements.Select(x => variables.Resolve(x, dependencyMode)).ToArray();
            
            return new ArrayAssignmentFrame<T>(this, elements).Variable;
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

        public int Hash { get; set; }
        
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