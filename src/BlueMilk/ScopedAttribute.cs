﻿using System;
using BlueMilk.IoC.Instances;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk
{
    /// <summary>
    /// Makes BlueMilk treat a Type as a singleton in the lifecycle scoping
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ScopedAttribute : BlueMilkAttribute
    {
        // This method will affect single registrations
        public override void Alter(IConfiguredInstance instance)
        {
            instance.Lifetime = ServiceLifetime.Scoped;
        }
    }
}