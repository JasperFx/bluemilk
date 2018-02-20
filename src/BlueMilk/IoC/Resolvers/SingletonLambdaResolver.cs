﻿using System;
using Baseline;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Resolvers
{
    public class SingletonLambdaResolver<TContainer, T> : SingletonResolver<T> 
    {
        private readonly Func<TContainer, T> _builder;
        
        public SingletonLambdaResolver(Func<TContainer, T> builder, Scope topLevelScope) : base(topLevelScope)
        {
            _builder = builder;
        }
        
        public override T Build(Scope scope)
        {
            return _builder(scope.As<TContainer>());
        }
    }
}