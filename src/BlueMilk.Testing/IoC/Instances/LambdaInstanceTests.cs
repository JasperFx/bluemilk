﻿using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace BlueMilk.Testing.IoC.Instances
{
    public class LambdaInstanceTests
    {
        private readonly ResolverGraph theResolvers = ResolverGraph.Empty();
        
        [Fact]
        public void derive_the_default_name()
        {
            LambdaInstance.For(s => new Clock())
                .Name.ShouldBe(nameof(Clock));
        }
        
        [Fact]
        public void register_resolver_for_transient_as_default()
        {
            var instance = LambdaInstance.For<IClock>(s => new Clock(), ServiceLifetime.Transient);
            instance.IsDefault = true;
            
            instance.BuildResolver(null, theResolvers, null)
                .ShouldBeOfType<TransientLambdaResolver<IClock>>();
        }
        
        [Fact]
        public void register_resolver_for_scoped_as_default()
        {
            var instance = LambdaInstance.For<IClock>(s => new Clock(), ServiceLifetime.Scoped);
            instance.IsDefault = true;
            
            instance.BuildResolver(null, theResolvers, null)
                .ShouldBeOfType<ScopedLambdaResolver<IClock>>();
        }
        
        [Fact]
        public void register_resolver_for_singleton_as_default()
        {
            var instance = LambdaInstance.For<IClock>(s => new Clock(), ServiceLifetime.Singleton);
            instance.IsDefault = true;
            
            instance.BuildResolver(null, theResolvers, null)
                .ShouldBeOfType<SingletonLambdaResolver<IClock>>();
        }
    }
}