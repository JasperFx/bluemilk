﻿using System.Reflection;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;

namespace BlueMilk.IoC.Instances
{
    public class CtorArg
    {
        public ParameterInfo Parameter { get; }
        public Instance Instance { get; }

        public CtorArg(ParameterInfo parameter, Instance instance)
        {
            Parameter = parameter;
            Instance = instance;

            if (instance.IsInlineDependency() || instance is LambdaInstance && instance.ServiceType.IsGenericType)
            {
                instance.Name = Parameter.Name;
            }
        }

        public Variable Resolve(ResolverVariables variables, BuildMode mode)
        {
            if (Instance.IsInlineDependency())
            {
                return Instance.CreateInlineVariable(variables);
            }
                
            var inner = variables.Resolve(Instance, mode);
            return Parameter.IsOptional 
                ? new OptionalArgumentVariable(inner, Parameter) 
                : inner;
        }
    }
}