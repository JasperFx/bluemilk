using System;

namespace BlueMilk.Codegen
{
    // TODO -- is this thing even used anymore?
    public interface IGenerates<T>
    {
        GeneratedType ToClass(GenerationRules rules);

        string SourceCode { get; set; }

        T Create(Type[] types, Func<Type, object> container);

        string TypeName { get; }
    }
}
