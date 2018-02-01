using System;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Lazy
{
    // TODO -- remove IResolver implementation
    public class FuncInstance<T> : Instance, IResolver
    {

        public FuncInstance() : base(typeof(Func<T>), typeof(Func<T>), ServiceLifetime.Transient)

        {
            Name = "func_of_" + typeof(T).NameInCode();
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            return new GetFuncFrame(this, typeof(T)).Variable;
        }
        

        public override bool RequiresServiceProvider { get; } = true;

        protected override IResolver buildResolver(Scope rootScope)
        {
            return this;
        }

        public override object Resolve(Scope scope, ServiceGraph services)
        {
            Func<T> func = scope.GetInstance<T>;

            return func;
        }

        public object Resolve(Scope scope)
        {
            Func<T> func = scope.GetInstance<T>;

            return func;
        }

        public int Hash { get; set; }
        
    }
}