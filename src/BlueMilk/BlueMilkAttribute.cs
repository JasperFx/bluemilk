using System;
using BlueMilk.IoC.Instances;

namespace BlueMilk
{
    /// <summary>
    /// Base class for custom configuration attributes
    /// </summary>
    public abstract class BlueMilkAttribute : Attribute
    {
        /// <summary>
        /// Make configuration alterations to a single IConfiguredInstance object
        /// </summary>
        /// <param name="instance"></param>    
        public virtual void Alter(IConfiguredInstance instance)
        {
        }

        /// <summary>
        /// Make configuration changes to the most generic form of Instance
        /// </summary>
        /// <param name="instance"></param>
        public virtual void Alter(Instance instance)
        {
            
        }
    }
}