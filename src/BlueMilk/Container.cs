using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk
{

    public class Container : IServiceProvider, IServiceScopeFactory
    {
        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IServiceScope CreateScope()
        {
            throw new NotImplementedException();
        }
    }

}