﻿using System;
using System.Linq;
using BlueMilk.IoC.Instances;
using BlueMilk.Scanning.Conventions;
using BlueMilk.Util;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk
{
    public class CloseGenericFamilyPolicy : IFamilyPolicy
    {
        
        
        public ServiceFamily Build(Type type, ServiceGraph graph)
        {
            if (!type.IsGenericType) return null;

            var basicType = type.GetGenericTypeDefinition();

            if (graph.HasFamily(basicType))
            {
                var basicFamily = graph.ResolveFamily(basicType);
                var templatedParameterTypes = type.GetGenericArguments();

                return basicFamily.CreateTemplatedClone(type, templatedParameterTypes.ToArray());
            }
            
            try
            {
                return tryToConnect(type, graph);
            }
            catch (Exception)
            {
                return tryToConnect(type, graph);
            }

        }
        

        
        private ServiceFamily tryToConnect(Type type, ServiceGraph graph)
        {
            // RIGHT HERE: do the connections thing HERE!
            var connectingTypes = graph.Services.ConnectedConcretions().Where(x => x.CanBeCastTo(type)).ToArray();
           
            
            if (connectingTypes.Any())
            {
                var instances = connectingTypes.Select(x => new ConstructorInstance(type, x, ServiceLifetime.Transient)).ToArray();

                return new ServiceFamily(type, instances);
            }

            // This is a problem right here. Need this to be exposed
            return graph.Families.Values.ToArray()
                .FirstOrDefault(x => type.IsAssignableFrom(x.ServiceType));
        }
    }
}