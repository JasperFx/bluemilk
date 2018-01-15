using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;
using BlueMilk.IoC.Enumerables;

namespace BlueMilk.IoC.Frames
{
    public class ArrayAssignmentFrame<T> : SyncFrame
    {
        public ArrayAssignmentFrame(ArrayInstance<T> instance, Variable[] elements)
        {
            Elements = elements;
            Variable = new ServiceVariable(instance, this);
            ElementType = typeof(T);
        }



        public Type ElementType { get; }

        public Variable[] Elements { get; }

        public Variable Variable { get; }

        public override void GenerateCode(GeneratedMethod method, ISourceWriter writer)
        {
            var elements = Elements.Select(x => x.Usage).Join(", ");
            writer.Write($"var {Variable.Usage} = new {ElementType.FullNameInCode()}[]{{{elements}}};");
            Next?.GenerateCode(method, writer);
        }

        public override IEnumerable<Variable> FindVariables(IMethodVariables chain)
        {
            return Elements;
        }
    }
}
