﻿using System;
using BlueMilk.IoC.Instances;

namespace BlueMilk
{
    public class EmptyFamilyPolicy : IFamilyPolicy
    {
        public ServiceFamily Build(Type type, ServiceGraph serviceGraph)
        {
            if (!type.IsGenericTypeDefinition) return new ServiceFamily(type, new Instance[0]);

            return null;
        }
    }
}