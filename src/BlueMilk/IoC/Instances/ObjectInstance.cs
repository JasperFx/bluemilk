﻿using System;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class ObjectInstance : Instance, IDisposable
    {
        public static ObjectInstance For<T>(T @object)
        {
            return new ObjectInstance(typeof(T), @object);
        }
        
        public ObjectInstance(Type serviceType, object service) : base(serviceType, service?.GetType() ?? serviceType, ServiceLifetime.Singleton)
        {
            Service = service;
            Name = service?.GetType().NameInCode() ?? serviceType.NameInCode();
        }

        public object Service { get; }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            return new InjectedServiceField(this);
        }

        public override object Resolve(Scope scope)
        {
            return Service;
        }

        public override object QuickResolve(Scope scope)
        {
            return Service;
        }

        public void Dispose()
        {
            (Service as IDisposable)?.Dispose();
        }

        public override string ToString()
        {
            return $"{nameof(Service)}: {Service} ('{Name}')";
        }
    }
}