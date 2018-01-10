﻿using System;

namespace BlueMilk
{
    /// <summary>
    /// Allows BlueMilk to fill in missing registrations by unknown plugin types
    /// at runtime
    /// </summary>
    public interface IFamilyPolicy
    {
        /// <summary>
        /// Allows you to create missing registrations for an unknown plugin type
        /// at runtime.
        /// Return null if this policy does not apply to the given type
        /// </summary>
        ServiceFamily Build(Type type, ServiceGraph serviceGraph);
    }
}