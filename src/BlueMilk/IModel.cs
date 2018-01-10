using System;

namespace BlueMilk
{
    /// <summary>
    /// Can be used to analyze and query the registrations and configuration
    /// of the running Container or Scope
    /// </summary>
    public interface IModel
    {
        /// <summary>
        ///     Retrieves the configuration for the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IServiceFamilyConfiguration For<T>();

        /// <summary>
        ///     Retrieves the configuration for the given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IServiceFamilyConfiguration For(Type type);

    }
}