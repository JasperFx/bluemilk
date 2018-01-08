using System.Collections.Generic;
using BlueMilk.Codegen;
using BlueMilk.Codegen.Frames;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;

namespace BlueMilk.IoC.Frames
{
    public class ScopedCreationFrame : SyncFrame
    {
        private readonly ConstructorFrame _ctor;
        private readonly Variable[] _scopedDependencies;
        private readonly IMethodVariables _parent;
        private Variable _scope;


        public ScopedCreationFrame(ConstructorFrame ctor, Variable[] scopedDependencies, IMethodVariables parent)
        {
            _ctor = ctor;
            
            
            _scopedDependencies = scopedDependencies;
            _parent = parent;
            
            
        }
        
        /*
         * Notes:
         * 1. Will need a special service variable type that has the lifetime
         * 2. ConstructorFrame
         * 3. Still need to be able to tell if a build step depends on any implementation factories
         *
         * 
         */

        public override IEnumerable<Variable> FindVariables(IMethodVariables chain)
        {
            _scope = chain.FindVariable(typeof(Scope));
            yield return _scope;

            // NO, you may not need this
            // You need this strictly for ordering
            foreach (var scopedDependency in _scopedDependencies)
            {
                yield return scopedDependency;
            }
        }

        public override void GenerateCode(GeneratedMethod method, ISourceWriter writer)
        {
            // TODO -- do the DependencyGatherer, but throw away any frame that creates a 
            // singleton or scoped dependency
        }
    }
}