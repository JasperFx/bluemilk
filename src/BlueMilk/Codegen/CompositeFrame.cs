﻿using System.Collections.Generic;
using System.Linq;
using BlueMilk.Compilation;

namespace BlueMilk.Codegen
{
    public abstract class CompositeFrame : Frame
    {
        private readonly Frame[] _inner;

        protected CompositeFrame(params Frame[] inner) : base(inner.Any(x => x.IsAsync))
        {
            _inner = inner;
        }

        public override IEnumerable<Variable> Creates => Enumerable.SelectMany<Frame, Variable>(_inner, x => x.Creates).ToArray();
        public sealed override void GenerateCode(IGeneratedMethod method, ISourceWriter writer)
        {
            if (_inner.Length > 1)
            {
                for (int i = 1; i < _inner.Length; i++)
                {
                    _inner[i - 1].Next = _inner[i];
                }
            }

            generateCode(method, writer, _inner[0]);

            Next?.GenerateCode(method, writer);
        }

        protected abstract void generateCode(IGeneratedMethod method, ISourceWriter writer, Frame inner);

        public override IEnumerable<Variable> FindVariables(IMethodVariables chain)
        {
            return _inner.SelectMany(x => x.FindVariables(chain)).Distinct();
        }

        public override bool CanReturnTask()
        {
            return _inner.Last().CanReturnTask();
        }
    }
}
