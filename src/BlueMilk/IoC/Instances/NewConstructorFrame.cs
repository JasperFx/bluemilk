using System;
using System.Collections.Generic;
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

        public NewConstructorFrame(ConstructorInstance instance, DisposeTracking disposal, Variable[] arguments)
        {
            Disposal = disposal;
            _arguments = arguments;
            Variable = new ServiceVariable(instance, this);
        }
        
        public ServiceVariable Variable { get; }

        public DisposeTracking Disposal { get; }

        public override void GenerateCode(GeneratedMethod method, ISourceWriter writer)
        {
            throw new NotImplementedException();
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