using System;
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
            var length = ctor.GetParameters().Length;
            var openType = _openTypes[length];

            var parameterTypes = ctor.GetParameters().Select(x => x.ParameterType).ToList();
            parameterTypes.Add(concreteType);

            var funcType = openType.MakeGenericType(parameterTypes.ToArray());

            var arguments = ctor.GetParameters().Select(param => Expression.Parameter(param.ParameterType)).ToArray();

            var callCtor = Expression.New(ctor, arguments);

            var lambda = Expression.Lambda(funcType, callCtor, arguments);
            var func = lambda.Compile();
            
            return (func, funcType);
        }
    }
}