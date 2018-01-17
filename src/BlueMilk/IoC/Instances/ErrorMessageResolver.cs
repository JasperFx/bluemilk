using System;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.IoC.Resolvers;

namespace BlueMilk.IoC.Instances
{
    public class ErrorMessageResolver : IResolver
    {
        private readonly string _message;

        public ErrorMessageResolver(Instance instance)
        {
            ServiceType = instance.ServiceType;
            Name = instance.Name;
            Hash = instance.GetHashCode();

            _message = instance.ErrorMessages.Join(Environment.NewLine);
        }

        public object Resolve(Scope scope)
        {
            throw new BlueMilkException($"Cannot build registered instance {Name} of '{ServiceType.FullNameInCode()}':{Environment.NewLine}{_message}");
        }

        public Type ServiceType { get; }
        public string Name { get; set; }
        public int Hash { get; set; }
    }
}