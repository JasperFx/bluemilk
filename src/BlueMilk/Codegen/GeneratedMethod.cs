using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.Compilation;

namespace BlueMilk.Codegen
{
    public abstract class GeneratedMethod : IGeneratedMethod
    {
        private readonly Argument[] _arguments;
        private readonly Dictionary<Type, Variable> _variables = new Dictionary<Type, Variable>();
        private AsyncMode _asyncMode = AsyncMode.AsyncTask;
        private Frame _top;

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

        public string MethodName { get; }
        public bool Overrides { get; set; }
        public bool Virtual { get; set; }
        public AsyncMode AsyncMode => _asyncMode;
        public InjectedField[] Fields { get; set; } = new InjectedField[0];
        public IList<Frame> Frames { get; }
        public IEnumerable<Argument> Arguments => _arguments;
        
        
        // TODO -- need a test here. It's used within Jasper, but still
        public Visibility Visibility { get; set; } = Visibility.Public;
        
        // TODO -- need a test here. It's used within Jasper, but still
        public IList<Variable> DerivedVariables { get; } = new List<Variable>();
        
        
        public IList<IVariableSource> Sources { get; } = new List<IVariableSource>();

        public void WriteMethod(ISourceWriter writer)
        {
            if (_top == null) throw new InvalidOperationException($"You must call {nameof(ArrangeFrames)}() before writing out the source code");
            
            var returnValue = determineReturnExpression();

            if (Overrides)
            {
                returnValue = "override " + returnValue;
            }

            var arguments = Arguments.Select(x => x.Declaration).Join(", ");

            writer.Write($"BLOCK:public {returnValue} {MethodName}({arguments})");


            _top.GenerateCode(this, writer);

            writeReturnStatement(writer);

            writer.FinishBlock();
        }

        protected abstract void writeReturnStatement(ISourceWriter writer);
        protected abstract string determineReturnExpression();

        public void ArrangeFrames(GeneratedClass @class)
        {
            var compiler = new MethodFrameArranger(this, @class);
            compiler.Arrange(out _asyncMode, out _top);
        }

        public abstract string ToExitStatement();
    }
}