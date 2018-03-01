﻿using System;
using System.Collections.Generic;
using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;
using BlueMilk.Util;

namespace BlueMilk.Codegen.ServiceLocation
{
    public class ServiceResolutionFrame : SyncFrame
    {
        private Variable _provider;

        public ServiceResolutionFrame(Type serviceType)
        {
            Service = new Variable(serviceType, this);
            
        }

        public Variable Service { get; }

        public override IEnumerable<Variable> FindVariables(IMethodVariables chain)
        {
            _provider = chain.FindVariable(typeof(IServiceProvider));
            yield return _provider;
        }

        public override void GenerateCode(GeneratedMethod method, ISourceWriter writer)
        {
            var typeFullName = Service.VariableType.FullNameInCode();
            var declaration = $"var {Service.Usage} = ({typeFullName}){_provider.Usage}.{nameof(IServiceProvider.GetService)}(typeof({typeFullName}))";

            if (Service.VariableType.CanBeCastTo<IDisposable>())
            {
                writer.UsingBlock(declaration, w => Next?.GenerateCode(method, w));
            }
            else
            {
                writer.Write(declaration + ";");
                Next?.GenerateCode(method, writer);
            }
        }
    }
}
