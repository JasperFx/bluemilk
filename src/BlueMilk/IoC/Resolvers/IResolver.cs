using System;

namespace BlueMilk.IoC.Resolvers
{
    public interface IResolver
    {
        object Resolve(Scope scope);
        Type ServiceType { get; }
    }
}