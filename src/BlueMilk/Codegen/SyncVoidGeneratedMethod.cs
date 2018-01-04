using System.Collections.Generic;
using BlueMilk.Compilation;

namespace BlueMilk.Codegen
{
    public class SyncVoidGeneratedMethod : GeneratedMethod<SyncVoidGeneratedMethod>
    {
        public SyncVoidGeneratedMethod(string methodName, Argument[] arguments, IList<Frame> frames = null) : base(methodName, arguments, frames)
        {
        }

        protected override void writeReturnStatement(ISourceWriter writer)
        {
            // nothing
        }

        protected override string determineReturnExpression()
        {
            return "void";
        }

        public override string ToExitStatement()
        {
            return string.Empty;
        }
    }
}