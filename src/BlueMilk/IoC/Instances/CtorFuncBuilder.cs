using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BlueMilk.IoC.Instances
{
    public static class CtorFuncBuilder
    {
        private static readonly Type[] _openTypes = new Type[]
        {
            typeof(Func<>),
            typeof(Func<,>),
            typeof(Func<,,>),
            typeof(Func<,,,>),
            typeof(Func<,,,,>),
            typeof(Func<,,,,,>),
            typeof(Func<,,,,,,>),
            typeof(Func<,,,,,,,>),
            typeof(Func<,,,,,,,,>),
            typeof(Func<,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,,,>)

        };

        public static (Delegate func, Type funcType) LambdaTypeFor(Type concreteType, ConstructorInfo ctor)
        {
            return LambdaTypeFor(concreteType, concreteType, ctor);
        }

        public static (Delegate func, Type funcType) LambdaTypeFor(Type serviceType, Type concreteType, ConstructorInfo ctor)
        {
            var length = ctor.GetParameters().Length;
            var openType = _openTypes[length];



            var parameters = ctor.GetParameters();
            var arguments = new ParameterExpression[parameters.Length];
            var ctorParams = new Expression[parameters.Length];

            var parameterTypes = new List<Type>();
            
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                if (parameter.ParameterType.IsPublic)
                {
                    ctorParams[i] = arguments[i] = Expression.Parameter(parameter.ParameterType);
                    parameterTypes.Add(parameter.ParameterType);
                }
                else
                {
                    arguments[i] = Expression.Parameter(typeof(object));
                    ctorParams[i] = Expression.Convert(arguments[i], parameter.ParameterType);
                    parameterTypes.Add(typeof(object));
                }
            }

            
            if (serviceType.IsPublic)
            {
                parameterTypes.Add(serviceType);
            }
            else
            {
                parameterTypes.Add(typeof(object));
            }
            
            var funcType = openType.MakeGenericType(parameterTypes.ToArray());

            
            var callCtor = Expression.New(ctor, ctorParams);

            var lambda = Expression.Lambda(funcType, callCtor, arguments);
            var func = lambda.Compile();
            
            return (func, funcType);
        }
    }
}