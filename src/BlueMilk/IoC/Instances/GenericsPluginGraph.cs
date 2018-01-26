﻿using System;
using System.Reflection;

namespace BlueMilk.IoC.Instances
{
    public static class GenericsPluginGraph
    {
        public static bool CanBeCast(Type serviceType, Type implementationType)
        {
            try
            {
                return checkGenericType(implementationType, serviceType);
            }
            catch (Exception e)
            {
                var message =
                    string.Format("Could not Determine Whether Type '{0}' plugs into Type '{1}'",
                        serviceType.Name,
                        implementationType.Name);
                throw new ArgumentException(message, e);
            }
        }

        private static bool checkGenericType(Type pluggedType, Type pluginType)
        {
            if (pluggedType == null || pluginType == null) return false;

            if (pluginType.GetTypeInfo().IsAssignableFrom(pluggedType.GetTypeInfo())) return true;


// check interfaces
            foreach (var type in pluggedType.GetInterfaces())
            {
                if (!type.GetTypeInfo().IsGenericType)
                {
                    continue;
                }

                if (type.GetGenericTypeDefinition() == pluginType)
                {
                    return true;
                }
            }

            var baseType = pluggedType.GetTypeInfo().BaseType;
            if (baseType != null && baseType.GetTypeInfo().IsGenericType)
            {
                var baseTypeGenericDefinition = baseType.GetGenericTypeDefinition();

                if (baseTypeGenericDefinition == pluginType)
                {
                    return true;
                }
                else
                {
                    return CanBeCast(pluginType, baseTypeGenericDefinition);
                }
            }

            return false;
        }
    }
}