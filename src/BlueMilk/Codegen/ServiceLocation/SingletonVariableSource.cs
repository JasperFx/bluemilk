﻿using System;
using BlueMilk.Codegen.Variables;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.Codegen.ServiceLocation
{
    public class SingletonVariableSource : IVariableSource
    {
        private readonly OldServiceGraph _graph;

        public SingletonVariableSource(OldServiceGraph graph)
        {
            _graph = graph;
        }

        public bool Matches(Type type)
        {
            if (type == typeof(IServiceScopeFactory)) return true;

            var descriptor = _graph.FindDefault(type);
            return descriptor?.Lifetime == ServiceLifetime.Singleton;
        }

        public Variable Create(Type type)
        {
            return new InjectedField(type);
        }
    }
}