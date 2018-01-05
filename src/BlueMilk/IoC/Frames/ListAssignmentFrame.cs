﻿using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;
using BlueMilk.IoC.Planning;

namespace BlueMilk.IoC.Frames
{
    public class ListAssignmentFrame : Frame
    {
        public ListAssignmentFrame(Type serviceType, Variable[] elements) : base(false)
        {
            ElementType = EnumerableStep.DetermineElementType(serviceType);
            Variable = new Variable(serviceType, Variable.DefaultArgName(ElementType) + "List", this);

            Elements = elements;
        }

        public Type ElementType { get; }

        public Variable[] Elements { get; }

        public Variable Variable { get; }

        public override void GenerateCode(IGeneratedMethod method, ISourceWriter writer)
        {
            var elements = Elements.Select(x => x.Usage).Join(", ");
            writer.Write($"var {Variable.Usage} = new {typeof(List<>).Namespace}.List<{ElementType.FullNameInCode()}>{{{elements}}};");
            Next?.GenerateCode(method, writer);
        }

        public override IEnumerable<Variable> FindVariables(IMethodVariables chain)
        {
            return Elements;
        }
    }
}
