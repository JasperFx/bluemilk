using System;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Frames;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.IoC.Instances
{
    public class ValueInstance : Instance
    {
        public static ValueInstance For<T>(T value)
        {
            return new ValueInstance(typeof(T), value);
        }
        
        public object Value { get; }

        public ValueInstance(Type serviceType, object value) : base(serviceType, serviceType, ServiceLifetime.Transient)
        {
            Value = value;
        }

        public override object Resolve(Scope scope)
        {
            return Value;
        }

        public override Variable CreateVariable(BuildMode mode, ResolverVariables variables, bool isRoot)
        {
            return new Variable(ServiceType, toUsage());
        }

        private string toUsage()
        {
            // TODO -- need to cleanse this a bit
            if (ImplementationType == typeof(string))
            {
                return $"\"{Value}\"";
            }
            
            if (ImplementationType == typeof(Guid))
            {
                return $"{typeof(Guid).FullName}.{nameof(Guid.Parse)}(\"{Value.ToString()}\")";
            }

            if (ImplementationType.IsEnum)
            {
                return $"{ImplementationType.FullNameInCode()}.{Value.ToString()}";
            }

            if (ImplementationType == typeof(bool))
            {
                return Value.ToString().ToLower();
            }

            return Value == null ? "null" : Value.ToString();

        }
    }
}