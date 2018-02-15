using System;
using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;

namespace BlueMilk.Codegen
{
    public class GetServiceFrame : SyncFrame
    {
        private readonly Variable _provider;

        public GetServiceFrame(Variable provider, Type serviceType)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            uses.Add(provider);

            Variable = new Variable(serviceType, this);
        }

        public Variable Variable { get; }

        public override void GenerateCode(GeneratedMethod method, ISourceWriter writer)
        {
            writer.Write($"var {Variable.Usage} = ({Variable.VariableType.FullNameInCode()}){_provider.Usage}.{nameof(IServiceProvider.GetService)}(typeof({Variable.VariableType.FullNameInCode()}));");
            Next?.GenerateCode(method, writer);
        }
    }
}