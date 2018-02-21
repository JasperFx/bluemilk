﻿using System;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using BlueMilk.IoC.Instances;
using BlueMilk.IoC.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Lazy
{
    public class FuncByNameInstance<T> : Instance
    {
        public FuncByNameInstance() : base(typeof(Func<string, T>), typeof(Func<string, T>), ServiceLifetime.Transient)
        {
            Name = "func_by_name_of_" + typeof(T).NameInCode();
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            return new GetFuncByNameFrame(this, typeof(T)).Variable;
        }
        

        public override bool RequiresServiceProvider { get; } = true;

        public override Func<Scope, object> ToResolver(Scope topScope)
        {
            return scope =>
            {
                T Func(string name) => scope.GetInstance<T>(name);

                return (Func<string, T>) Func;
            };
        }

        public override object Resolve(Scope scope)
        {
            T Func(string name) => scope.GetInstance<T>(name);

            return (Func<string, T>) Func;
        }
    }
}