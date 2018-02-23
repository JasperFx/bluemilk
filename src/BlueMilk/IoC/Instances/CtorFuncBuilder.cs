using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.IoC.Enumerables;
using BlueMilk.Scanning.Conventions;
using FastExpressionCompiler;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace BlueMilk.IoC.Instances
{
    public static class CtorFuncBuilder
    {
        static CtorFuncBuilder()
        {
            _coerceToList = typeof(CtorFuncBuilder).GetMethod(nameof(CoerceToList));
            _coerceToArray = typeof(CtorFuncBuilder).GetMethod(nameof(CoerceArray));
        }
        
        public static IList<T> CoerceToList<T>(object elements)
        {
            return elements.As<object[]>().OfType<T>().ToList();
        }

        public static T[] CoerceArray<T>(object elements)
        {
            return elements.As<object[]>().OfType<T>().ToArray();
        }
        
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

        private static MethodInfo _coerceToList;
        private static MethodInfo _coerceToArray;

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

                if (parameter.ParameterType.MustBeBuiltWithFunc())
                {
                    arguments[i] = Expression.Parameter(typeof(object));

                    if (EnumerablePolicy.IsEnumerable(parameter.ParameterType))
                    {
                        var elementType = EnumerablePolicy.DetermineElementType(parameter.ParameterType);
                        var coerceMethod = (parameter.ParameterType.IsArray ? _coerceToArray : _coerceToList)
                            .MakeGenericMethod(elementType);
                        
                        ctorParams[i] = Expression.Call(coerceMethod, arguments[i]);
                    }
                    else
                    {
                        ctorParams[i] = Expression.Convert(arguments[i], parameter.ParameterType);
                    }

                    parameterTypes.Add(typeof(object));
                }
                else
                {
                    ctorParams[i] = arguments[i] = Expression.Parameter(parameter.ParameterType);
                    parameterTypes.Add(parameter.ParameterType);
                }
            }

            var parameterType = serviceType.MustBeBuiltWithFunc() ? typeof(object) : serviceType;
            parameterTypes.Add(parameterType);

            var funcType = openType.MakeGenericType(parameterTypes.ToArray());
            
            if (funcType.MustBeBuiltWithFunc()) throw new InvalidOperationException($"The Func of signature {funcType.NameInCode()} is not publicly accessible for type {serviceType.FullNameInCode()}");

            
            var callCtor = Expression.New(ctor, ctorParams);

            var lambda = Expression.Lambda(funcType, callCtor, arguments);
            var func = lambda.CompileFast();
            
            return (func, funcType);
        }
    }
}