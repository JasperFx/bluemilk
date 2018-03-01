using System;
using System.Collections.Generic;
using System.Linq;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;
using BlueMilk.IoC.Enumerables;
using BlueMilk.Scanning.Conventions;
using BlueMilk.Util;

namespace BlueMilk.IoC.Frames
{
    public class ArrayAssignmentFrame<T> : SyncFrame
    {
        public ArrayAssignmentFrame(ArrayInstance<T> instance, Variable[] elements)
        {
            Elements = elements;
            Variable = new ServiceVariable(instance, this);
            if (typeof(T).MustBeBuiltWithFunc())
            {
                Variable.OverrideType(typeof(object[]));
            }
            
            ElementType = typeof(T);
        }



        public Type ElementType { get; }

        public Variable[] Elements { get; }

        public Variable Variable { get; }
        public bool ReturnCreated { get; set; }

        public override void GenerateCode(GeneratedMethod method, ISourceWriter writer)
        {
            var elements = Elements.Select(x => x.Usage).Join(", ");

            var arrayType = ElementType.MustBeBuiltWithFunc() ? "object" : ElementType.FullNameInCode();

            if (ReturnCreated)
            {
                writer.Write($"return new {arrayType}[]{{{elements}}};");
            }
            else
            {
                writer.Write($"var {Variable.Usage} = new {arrayType}[]{{{elements}}};");
            }
            
            
            Next?.GenerateCode(method, writer);
        }

        public override IEnumerable<Variable> FindVariables(IMethodVariables chain)
        {
            return Elements;
        }
    }
}
