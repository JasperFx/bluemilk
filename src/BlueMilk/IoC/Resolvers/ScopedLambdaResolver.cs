using System;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Resolvers
{
    public class ScopedLambdaResolver<T> : ScopedResolver<T>
    {
        private Func<IServiceProvider, object> _builder;

        public ScopedLambdaResolver(ServiceDescriptor descriptor)
        {
            _builder = descriptor.ImplementationFactory;
        }

        public override object Build(Scope scope)
        {
            throw new NotImplementedException();
        }
    }
}