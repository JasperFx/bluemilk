using System;
using System.Linq.Expressions;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class DefaultInstance : Instance
    {
        public DefaultInstance(Type serviceType, Type implementationType, ServiceLifetime lifetime) : base(serviceType, implementationType, lifetime)
        {
        }

        public DefaultInstance(Type serviceType) : base(serviceType, serviceType, ServiceLifetime.Transient)
        {
        }

        public override object Resolve(Scope scope)
        {
            throw new NotSupportedException();
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            throw new NotSupportedException();
        }
    }

    public class ReferencedInstance : Instance
    {
        public ReferencedInstance(Type serviceType, Type implementationType, ServiceLifetime lifetime) : base(serviceType, implementationType, lifetime)
        {
        }

        public ReferencedInstance(Type serviceType, string instanceKey) : base(serviceType, serviceType, ServiceLifetime.Transient)
        {
        }

        public override object Resolve(Scope scope)
        {
            throw new NotSupportedException();
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            throw new NotSupportedException();
        }
    }
    
    /*
    /// <summary>
    /// Expression Builder that helps to define child dependencies inline 
    /// </summary>
    public class DependencyExpression<TChild> 
    {
        private readonly ConstructorInstance _instance;
        private readonly string _propertyName;

        internal DependencyExpression(ConstructorInstance instance, string propertyName)
        {
            _instance = instance;
            _propertyName = propertyName;
        }


        /// <summary>
        /// Inline dependency by simple Lambda expression
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public ConstructorInstance Is(Func<IContainer, TChild> func)
        {
            var child = new LambdaInstance<TChild>(func);
            return Is(child);
        }


        /// <summary>
        /// Inline dependency by Lambda expression that uses IContext
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public ConstructorInstance Is(Func<IContainer, TChild> func)
        {
            var child = new LambdaInstance(func);
            return Is(child);
        }

        /// <summary>
        /// Inline dependency by Lambda Func that uses IContext
        /// </summary>
        /// <param name="description">User friendly description for diagnostics</param>
        /// <param name="func"></param>
        /// <returns></returns>
        public ConstructorInstance Is(string description, Func<IContainer, TChild> func)
        {
            var child = new LambdaInstance<TChild>(description, func);
            return Is(child);
        }

        /// <summary>
        /// Shortcut to set an inline dependency to an Instance
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public ConstructorInstance Is(Instance instance)
        {
            _instance.Dependencies.Add(_propertyName, typeof (TChild), instance);
            return _instance;
        }

        /// <summary>
        /// Shortcut to set an inline dependency to a designated object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ConstructorInstance Is(TChild value)
        {
            _instance.Dependencies.Add(_propertyName, typeof (TChild), value);
            return _instance;
        }

        /// <summary>
        /// Set an Inline dependency to the Default Instance of the Property type
        /// Used mostly to force an optional Setter property to be filled by
        /// StructureMap
        /// </summary>
        /// <returns></returns>
        public ConstructorInstance IsTheDefault()
        {
            return Is(new DefaultInstance(typeof(TChild)));
        }

        /// <summary>
        /// Set the inline dependency to the named instance of the property type
        /// Used mostly to force an optional Setter property to be filled by
        /// StructureMap        /// </summary>
        /// <param name="instanceKey"></param>
        /// <returns></returns>
        public ConstructorInstance IsNamedInstance(string instanceKey)
        {
            return Is(new ReferencedInstance(typeof(TChild), instanceKey));
        }

        /// <summary>
        /// Shortcut method to define a child dependency inline
        /// </summary>
        /// <typeparam name="TConcreteType"></typeparam>
        /// <returns></returns>
        public ConstructorInstance Is<TConcreteType>() where TConcreteType : TChild
        {
            return Is(new ConstructorInstance<TConcreteType>(typeof(TChild), ServiceLifetime.Transient);
        }


        /// <summary>
        /// Shortcut method to define a child dependency inline and configure
        /// the child dependency
        /// </summary>
        /// <typeparam name="TConcreteType"></typeparam>
        /// <returns></returns>
        public ConstructorInstance Is<TConcreteType>(Action<ConstructorInstance<TConcreteType>> configure) where TConcreteType : TChild
        {
            var instance = new ConstructorInstance<TConcreteType>(typeof(TChild), ServiceLifetime.Transient);
            configure(instance);
            return Is(instance);
        }

        /// <summary>
        /// Use the named Instance of TChild for the inline
        /// dependency here
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ConstructorInstance Named(string name)
        {
            return Is(c => c.GetInstance<TChild>(name));
        }
    }
    */
}