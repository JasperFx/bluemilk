using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Planning;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Resolvers
{
    public class ScopeInstance<T> : Instance, IResolver
    {
        private readonly Variable _variable;

        public ScopeInstance() : base(typeof(T), typeof(T), ServiceLifetime.Scoped)
        {
            _variable = new CastScopeFrame(typeof(T)).Variable;
            Name = typeof(T).Name;
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            return _variable;
        }

        protected override IResolver buildResolver(Scope rootScope)
        {
            return this;
        }

        public object Resolve(Scope scope)
        {
            return scope;
        }

        public int Hash { get; set; }
    }
}