using System;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Resolvers
{
    public class ScopedLambdaResolver<T> : ScopedResolver<T>
    {
        private readonly Func<IServiceProvider, object> _builder;

        public ScopedLambdaResolver(Func<IServiceProvider, object> builder)
        {
            _builder = builder;
        }

        public override object Build(Scope scope)
        {
            return _builder(scope.ServiceProvider);
        }
    }
}