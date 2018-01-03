using System;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Resolvers
{
    public class SingletonLambdaResolver<T> : SingletonResolver<T> where T : class
    {
        private readonly Func<IServiceProvider, object> _builder;
        
        public SingletonLambdaResolver(ServiceDescriptor descriptor, Scope topLevelScope) : base(topLevelScope)
        {
            _builder = descriptor.ImplementationFactory;
        }
        
        public override T Build(Scope scope)
        {
            return (T) _builder(scope.ServiceProvider);
        }
    }
}