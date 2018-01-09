using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;
using BlueMilk.IoC.Frames;

namespace BlueMilk.IoC.Instances
{
    public class NewConstructorFrame : SyncFrame
    {
        private readonly Variable[] _arguments;
        private Variable _scope;
        private Type _implementationType;

        public NewConstructorFrame(ConstructorInstance instance, DisposeTracking disposal, Variable[] arguments)
        {
            Disposal = disposal;
            _arguments = arguments;
            Variable = new ServiceVariable(instance, this);
            _implementationType = instance.ImplementationType;
        }
        
        public ServiceVariable Variable { get; }

        public DisposeTracking Disposal { get; }

        public override void GenerateCode(GeneratedMethod method, ISourceWriter writer)
        {
            var arguments = _arguments.Select(x => x.Usage).Join(", ");
            var implementationTypeName = _implementationType.FullNameInCode();

            var declaration = $"var {Variable.Usage} = new {implementationTypeName}({arguments})";

            switch (Disposal)
            {
                case DisposeTracking.None:
                    writer.Write(declaration + ";");
                    Next?.GenerateCode(method, writer);
                    break;
                    
                case DisposeTracking.WithUsing:
                    if (Next is NewConstructorFrame && Next.As<NewConstructorFrame>().Disposal == DisposeTracking.WithUsing)
                    {
                        writer.Write($"using ({declaration})");
                        Next?.GenerateCode(method, writer);
                    }
                    else
                    {
                        writer.UsingBlock(declaration, w => Next?.GenerateCode(method, w));
                    }

                    break;
                    
                case DisposeTracking.RegisterWithScope:
                    writer.Write(declaration + ";");
                    writer.Write($"{_scope.Usage}.{nameof(Scope.RegisterDisposable)}({Variable.Usage});");
                    Next?.GenerateCode(method, writer);
                    break;
                   
            }
            
            
        }

        public override IEnumerable<Variable> FindVariables(IMethodVariables chain)
        {
            foreach (var argument in _arguments)
            {
                yield return argument;
            }

            if (Disposal == DisposeTracking.RegisterWithScope)
            {
                _scope = chain.FindVariable(typeof(Scope));
                yield return _scope;
            }
        }
    }
}