using System;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Lazy
{
    // TODO -- remove the IResolver implementation
    public class LazyInstance<T> : Instance, IResolver
    {

        public LazyInstance() : base(typeof(Lazy<T>), typeof(Lazy<T>), ServiceLifetime.Transient)
        {
            Name = "lazy_of_" + typeof(T).NameInCode();
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            return new GetLazyFrame(this, typeof(T)).Variable;
        }
        

        public override bool RequiresServiceProvider { get; } = true;

        protected override IResolver buildResolver(Scope rootScope)
        {
            return this;
        }

        public override object Resolve(Scope scope, ServiceGraph services)
        {
            return new Lazy<T>(scope.GetInstance<T>);
        }

        public object Resolve(Scope scope)
        {
            return new Lazy<T>(scope.GetInstance<T>);
        }

        public int Hash { get; set; }

    }
}