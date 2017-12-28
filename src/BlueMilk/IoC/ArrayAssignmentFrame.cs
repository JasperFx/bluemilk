﻿using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.Compilation;

namespace BlueMilk.IoC
{
    public class ArrayAssignmentFrame : Frame
    {
        public ArrayAssignmentFrame(Type elementType, Variable[] elements) : base(false)
        {
            ElementType = elementType;
            Variable = new Variable(elementType.MakeArrayType(), Variable.DefaultArgName(elementType) + "Array", this);

            Elements = elements;
        }

        public Type ElementType { get; }

        public Variable[] Elements { get; }

        public Variable Variable { get; }

        public override void GenerateCode(IGeneratedMethod method, ISourceWriter writer)
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
