using System;
using System.Collections;
using System.Collections.Generic;

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
        /// Provides queryable access to the configured PluginType's and Instances of this Container.
        /// </summary>
        IModel Model { get; }

    }
}