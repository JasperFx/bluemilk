using System.Collections.Generic;
using System.Threading.Tasks;
using BlueMilk.Compilation;

namespace BlueMilk.Codegen
{
    public class GeneratedTaskMethod : GeneratedMethod<GeneratedTaskMethod>
    {
        public GeneratedTaskMethod(string methodName, Argument[] arguments, IList<Frame> frames) : base(methodName, arguments, frames)
        {
        }


        protected override void writeReturnStatement(ISourceWriter writer)
        {
            if (AsyncMode == AsyncMode.ReturnCompletedTask)
            {
                writer.Write("return Task.CompletedTask;");
            }
        }

        protected override string determineReturnExpression()
        {
            return AsyncMode == AsyncMode.AsyncTask
                ? "async Task"
                : "Task";
        }

        public override string ToExitStatement()
        {
            return AsyncMode == AsyncMode.AsyncTask 
                ? "return;" 
                : $"return {typeof(Task).FullName}.{nameof(Task.CompletedTask)};";
        }
    }
}

