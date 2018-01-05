using System;
using System.Collections.Generic;
using BlueMilk.Codegen.Frames;
using BlueMilk.Compilation;

namespace BlueMilk.Codegen.Methods
{
    public class SyncSingleReturnGeneratedMethod : GeneratedMethod<SyncSingleReturnGeneratedMethod>
    {
        public Type ReturnType { get; }

        public SyncSingleReturnGeneratedMethod(string methodName, Type returnType, Argument[] arguments, IList<Frame> frames = null) : base(methodName, arguments, frames)
        {
            ReturnType = returnType;
        }

        protected override void writeReturnStatement(ISourceWriter writer)
        {
            // Nothing
        }

        protected override string determineReturnExpression()
        {
            return ReturnType.FullNameInCode();
        }

        public override string ToExitStatement()
        {
            throw new System.NotImplementedException();
        }
    }
}