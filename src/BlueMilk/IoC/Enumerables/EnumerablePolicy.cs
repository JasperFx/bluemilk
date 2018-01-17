﻿using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.IoC.Instances;

namespace BlueMilk.IoC.Enumerables
{
    public class EnumerablePolicy : IFamilyPolicy
    {
        public static bool IsEnumerable(Type type)
        {
            if (type.IsArray) return true;

            return type.IsGenericType && _enumerableTypes.Contains(type.GetGenericTypeDefinition());
        }

        public static Type DetermineElementType(Type serviceType)
        {
            if (serviceType.IsArray)
            {
                return serviceType.GetElementType();
            }

            return serviceType.GetGenericArguments().First();
        }
        
        private static readonly List<Type> _enumerableTypes = new List<Type>
        {
            typeof (IEnumerable<>),
            typeof (IList<>),
            typeof (IReadOnlyList<>),
            typeof (List<>)
        };
        
        public ServiceFamily Build(Type type, ServiceGraph serviceGraph)
        {
            if (type.IsArray)
            {
                var instanceType = typeof(ArrayInstance<>).MakeGenericType(type.GetElementType());
                var instance = Activator.CreateInstance(instanceType, type).As<Instance>();
                return new ServiceFamily(type, instance);
            }

            if (type.IsGenericType && _enumerableTypes.Contains(type.GetGenericTypeDefinition()))
            {
                var elementType = type.GetGenericArguments().First();
                
                var instanceType = typeof(ListInstance<>).MakeGenericType(elementType);
                var instance = Activator.CreateInstance(instanceType, type).As<Instance>();
                return new ServiceFamily(type, instance);
            }

            return null;
        }
    }
}