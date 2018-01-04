using System.Collections.Generic;
using BlueMilk.Compilation;

namespace BlueMilk.Codegen
{
    public interface IGeneratedMethod
    {
        string MethodName { get; }
        bool Virtual { get; set; }
        AsyncMode AsyncMode { get; }
        IEnumerable<Argument> Arguments { get; }
        InjectedField[] Fields { get; set; }
        IList<Frame> Frames { get; }
        IList<IVariableSource> Sources { get; }
        IList<Variable> DerivedVariables { get; }
        void WriteMethod(ISourceWriter writer);
        void ArrangeFrames(GeneratedClass @class);
    }
}