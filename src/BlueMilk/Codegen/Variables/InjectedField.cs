﻿using System;
using BlueMilk.Compilation;

namespace BlueMilk.Codegen.Variables
{
    public class InjectedField : Variable
    {
        public InjectedField(Type argType) : this(argType, DefaultArgName(argType))
        {
        }

        public InjectedField(Type argType, string name) : base(argType, "_" + name)
        {
            CtorArg = name;
            ArgType = argType;
        }

        public Type ArgType { get; }

        public string CtorArg { get; protected set; }

        public virtual string CtorArgDeclaration => $"{ArgType.NameInCode()} {CtorArg}";

        public void WriteDeclaration(ISourceWriter writer)
        {
            writer.Write($"private readonly {ArgType.NameInCode()} {Usage};");
        }

        public void WriteAssignment(ISourceWriter writer)
        {
            writer.Write($"{Usage} = {CtorArg};");
        }
    }
}