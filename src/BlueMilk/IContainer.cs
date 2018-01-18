﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace BlueMilk
{
    public interface IContainer : IDisposable
    {
        /// <summary>
        /// Suitable for building concrete types that will be resolved only a few times
        /// to avoid the cost of having to register or build out a pre-compiled "build plan"
        /// internally
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T QuickBuild<T>();
        
        /// <summary>
        /// Creates or finds the default instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type which instance is to be created or found.</typeparam>
        /// <returns>The default instance of <typeparamref name="T"/>.</returns>
        T GetInstance<T>();

        /// <summary>
        /// Creates or finds the named instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type which instance is to be created or found.</typeparam>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The named instance of <typeparamref name="T"/>.</returns>
        T GetInstance<T>(string name);

        /// <summary>
        /// Creates or finds the default instance of <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type which instance is to be created or found.</param>
        /// <returns>The default instance of <paramref name="serviceType"/>.</returns>
        object GetInstance(Type serviceType);

        /// <summary>
        /// Creates or finds the named instance of <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type which instance is to be created or found.</param>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The named instance of <paramref name="serviceType"/>.</returns>
        object GetInstance(Type serviceType, string name);
        
        /// <summary>
        /// Starts a "Nested" Container for atomic, isolated access.
        /// </summary>
        /// <returns>The created nested container.</returns>
        IContainer GetNestedContainer();

        /// <summary>
        /// Creates or resolves all registered instances of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type which instances are to be created or resolved.</typeparam>
        /// <returns>All created or resolved instances of type <typeparamref name="T"/>.</returns>
        IReadOnlyList<T> GetAllInstances<T>();

        /// <summary>
        /// Creates or resolves all registered instances of the <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">The type which instances are to be created or resolved.</param>
        /// <returns>All created or resolved instances of type <paramref name="serviceType"/>.</returns>
        IEnumerable GetAllInstances(Type serviceType);
        
        /// <summary>
        /// Provides queryable access to the configured serviceType's and Instances of this Container.
        /// </summary>
        IModel Model { get; }
        
        /// <summary>
        /// Govern the behavior on Dispose() to prevent applications from 
        /// being prematurely disposed
        /// </summary>
        DisposalLock DisposalLock { get; set; }
        
                /// <summary>
        /// Creates or finds the default instance of <typeparamref name="T"/>. Returns the default value of
        /// <typeparamref name="T"/> if it is not known to the container.
        /// </summary>
        /// <typeparam name="T">The type which instance is to be created or found.</typeparam>
        /// <returns>The default instance of <typeparamref name="T"/> if resolved; the default value of
        /// <typeparamref name="T"/> otherwise.</returns>
        T TryGetInstance<T>();

        /// <summary>
        /// Creates or finds the named instance of <typeparamref name="T"/>. Returns the default value of
        /// <typeparamref name="T"/> if the named instance is not known to the container.
        /// </summary>
        /// <typeparam name="T">The type which instance is to be created or found.</typeparam>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The named instance of <typeparamref name="T"/> if resolved; the default value of
        /// <typeparamref name="T"/> otherwise.</returns>
        T TryGetInstance<T>(string name);

        /// <summary>
        /// Creates or finds the default instance of <paramref name="serviceType"/>. Returns <see langword="null"/> if
        /// <paramref name="serviceType"/> is not known to the container.
        /// </summary>
        /// <param name="serviceType">The type which instance is to be created or found.</param>
        /// <returns>The default instance of <paramref name="serviceType"/> if resolved; <see langword="null"/> otherwise.
        /// </returns>
        object TryGetInstance(Type serviceType);

        /// <summary>
        /// Creates or finds the named instance of <paramref name="serviceType"/>. Returns <see langword="null"/> if
        /// the named instance is not known to the container.
        /// </summary>
        /// <param name="serviceType">The type which instance is to be created or found.</param>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The named instance of <paramref name="serviceType"/> if resolved; <see langword="null"/> otherwise.
        /// </returns>
        object TryGetInstance(Type serviceType, string name);


        /// <summary>
        /// Returns a report detailing the complete configuration of all PluginTypes and Instances
        /// </summary>
        /// <param name="serviceType">Optional parameter to filter the results down to just this plugin type.</param>
        /// <param name="assembly">Optional parameter to filter the results down to only plugin types from this
        /// <see cref="Assembly"/>.</param>
        /// <param name="namespace">Optional parameter to filter the results down to only plugin types from this
        /// namespace.</param>
        /// <param name="typeName">Optional parameter to filter the results down to any plugin type whose name contains
        ///  this text.</param>
        /// <returns>The detailed report of the configuration.</returns>
        string WhatDoIHave(Type serviceType = null, Assembly assembly = null, string @namespace = null,
            string typeName = null);

        /// <summary>
        /// Returns a textual report of all the assembly scanners used to build up this Container
        /// </summary>
        /// <returns></returns>
        string WhatDidIScan();
    }
    
    public enum DisposalLock
    {
        /// <summary>
        /// If a user calls IContainer.Dispose(), ignore the request
        /// </summary>
        Ignore,

        /// <summary>
        /// Default "just dispose the container" behavior
        /// </summary>
        Unlocked,

        /// <summary>
        /// Throws an InvalidOperationException when Dispose() is called
        /// </summary>
        ThrowOnDispose
    }
}