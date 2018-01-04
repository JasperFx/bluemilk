using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
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

    public class GeneratedMethod : IGeneratedMethod
    {
        private readonly Argument[] _arguments;
        private readonly Dictionary<Type, Variable> _variables = new Dictionary<Type, Variable>();

        public string MethodName { get; }

        public GeneratedMethod(string methodName, Argument[] arguments, IList<Frame> frames)
        {
            if (!frames.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(frames), "Cannot be an empty list");
            }

            _arguments = arguments;
            MethodName = methodName;
            Frames = frames;
        }


        public bool Overrides { get; set; }
        public bool Virtual { get; set; }

        public void WriteMethod(ISourceWriter writer)
        {
            var returnValue = determineReturnExpression();

            if (Overrides)
            {
                returnValue = "override " + returnValue;
            }

            var arguments = Arguments.Select(x => x.Declaration).Join(", ");

            writer.Write($"BLOCK:public {returnValue} {MethodName}({arguments})");


            Top.GenerateCode(this, writer);

            writeReturnStatement(writer);

            writer.FinishBlock();
        }

        protected void writeReturnStatement(ISourceWriter writer)
        {
            if (AsyncMode == AsyncMode.ReturnCompletedTask)
            {
                writer.Write("return Task.CompletedTask;");
            }
        }

        protected string determineReturnExpression()
        {
            var returnValue = AsyncMode == AsyncMode.AsyncTask
                ? "async Task"
                : "Task";
            return returnValue;
        }

        public void ArrangeFrames(GeneratedClass @class)
        {
            var compiler = new MethodFrameArranger(this, @class);
            compiler.Arrange(out _asyncMode, out _top);
        }

        public virtual string ToExitStatement()
        {
            if (AsyncMode == AsyncMode.AsyncTask) return "return;";

            return $"return {typeof(Task).FullName}.{nameof(Task.CompletedTask)};";
        }

        public AsyncMode AsyncMode => _asyncMode;

        public Frame Top => _top;

        public InjectedField[] Fields { get; set; } = new InjectedField[0];

        public IList<Frame> Frames { get; }

        public IEnumerable<Argument> Arguments => _arguments;

        // TODO -- need a test here. It's used within Jasper, but still
        public Visibility Visibility { get; set; } = Visibility.Public;

        // TODO -- need a test here. It's used within Jasper, but still
        public IList<Variable> DerivedVariables { get; } = new List<Variable>();

        public IList<IVariableSource> Sources { get; } = new List<IVariableSource>();
        private AsyncMode _asyncMode = AsyncMode.AsyncTask;
        private Frame _top;
    }
}

