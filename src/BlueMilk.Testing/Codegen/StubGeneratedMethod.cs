using System.Collections.Generic;
using BlueMilk.Codegen;

namespace BlueMilk.Testing.Codegen
{
    public class StubGeneratedMethod : IGeneratedMethod
    {
        public string MethodName { get; set; }

        public bool Virtual { get; set; }

        public AsyncMode AsyncMode { get; set; } = AsyncMode.ReturnFromLastNode;

        public IEnumerable<Argument> Arguments { get; set; } = new Argument[0];
    }
}