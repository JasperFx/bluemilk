using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baseline;
using BlueMilk.Codegen.Frames;
using BlueMilk.Compilation;

namespace BlueMilk.Codegen.Methods
{
    public class AsyncSingleReturnGeneratedMethod : GeneratedMethod<SyncSingleReturnGeneratedMethod>
    {
        public static AsyncSingleReturnGeneratedMethod For<T>(string name, params Argument[] arguments)
        {
            var returnType = typeof(Task<T>);
            return new AsyncSingleReturnGeneratedMethod(name, returnType, arguments);
        }
        
        public Type ReturnType { get; }

        public AsyncSingleReturnGeneratedMethod(string methodName, Type returnType, Argument[] arguments, IList<Frame> frames = null) : base(methodName, arguments, frames)
        {
            if (!returnType.Closes(typeof(Task<>)))
            {
                throw new ArgumentOutOfRangeException(nameof(returnType), "Return type must close Task<T>");
            }
            
            ReturnType = returnType;
        }

        protected override void writeReturnStatement(ISourceWriter writer)
        {
            // Nothing
        }

        protected override string determineReturnExpression()
        {
            return AsyncMode == AsyncMode.AsyncTask
                ? "async " + ReturnType.FullNameInCode()
                : ReturnType.FullNameInCode();
        }

        public override string ToExitStatement()
        {
            throw new System.NotImplementedException();
        }
    }
}