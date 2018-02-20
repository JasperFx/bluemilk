﻿using System;
using Baseline;
using BlueMilk.IoC.Instances;

namespace BlueMilk
{
    // SAMPLE: IInstancePolicy
    /// <summary>
    /// Custom policy on Instance construction that is evaluated
    /// as part of creating a "build plan"
    /// </summary>
    
    public interface IInstancePolicy
    {
        /// <summary>
        /// Apply any conventional changes to the configuration
        /// of a single Instance
        /// </summary>
        /// <param name="instance"></param>
        void Apply(Instance instance);
    }
    // ENDSAMPLE


    /// <summary>
    /// Base class for using policies against IConfiguredInstance registrations
    /// </summary>
    public abstract class ConfiguredInstancePolicy : IInstancePolicy
    {
        public void Apply(Instance instance)
        {
            if (instance is IConfiguredInstance)
            {
                apply(instance.As<IConfiguredInstance>());
            }
        }

        protected abstract void apply(IConfiguredInstance instance);
    }
}