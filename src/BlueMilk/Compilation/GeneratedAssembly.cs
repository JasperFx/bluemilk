using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
using BlueMilk.Codegen;

namespace BlueMilk.Compilation
{
    
    /*
     * TODO -- try to use this class. Make it simpler if need be.
     *
     *
     *
     * 
     */
    public class GeneratedAssembly
    {
        private readonly GenerationRules _generation;
        public readonly List<GeneratedType> GeneratedTypes = new List<GeneratedType>();

        public GeneratedAssembly(GenerationRules generation)
        {
            _generation = generation;
        }

        public GeneratedType AddType(string typeName, Type baseType)
        {
            // TODO -- assert that it's been generated already?
            
            var generatedType = new GeneratedType(_generation, typeName);
            generatedType.BaseType = baseType;
            
            GeneratedTypes.Add(generatedType);

            return generatedType;
        }

        public void CompileAll()
        {
            var code = GenerateCode(_generation);

            var generator = buildGenerator(_generation);

            var assembly = generator.Generate(code);

            var generated = assembly.GetExportedTypes().ToArray();

            foreach (var generatedType in GeneratedTypes)
            {
                generatedType.FindType(generated);
            }
        }
        
        public string GenerateCode(GenerationRules generation)
        {
            foreach (var generatedType in GeneratedTypes)
            {
                generatedType.ArrangeFrames();
            }
            
            var namespaces = GeneratedTypes
                .SelectMany(x => x.Args())
                .Select(x => x.ArgType.Namespace)
                .Concat(new string[]{typeof(Task).Namespace})
                .Distinct().ToList();

            var writer = new SourceWriter();

            foreach (var ns in namespaces.OrderBy(x => x))
            {
                writer.Write($"using {ns};");
            }

            writer.BlankLine();

            writer.Namespace(generation.ApplicationNamespace);

            foreach (var @class in GeneratedTypes)
            {
                writer.WriteLine($"// START: {@class.TypeName}");
                @class.Write(writer);
                writer.WriteLine($"// END: {@class.TypeName}");

                writer.WriteLine("");
                writer.WriteLine("");
            }

            writer.FinishBlock();


            var code = writer.Code();

            attachSourceCodeToChains(code);


            return code;
        }
        
        private AssemblyGenerator buildGenerator(GenerationRules generation)
        {
            var generator = new AssemblyGenerator();
            generator.ReferenceAssembly(GetType().Assembly);
            generator.ReferenceAssembly(typeof(Task).Assembly);

            foreach (var assembly in generation.Assemblies)
            {
                generator.ReferenceAssembly(assembly);
            }

            GeneratedTypes
                .SelectMany(x => x.AssemblyReferences())
                .Distinct()
                .Each(x => generator.ReferenceAssembly(x));

            return generator;
        }
        
        private void attachSourceCodeToChains(string code)
        {
            var parser = new SourceCodeParser(code);
            foreach (var type in GeneratedTypes)
            {
                type.SourceCode = parser.CodeFor(type.TypeName);
            }
        }
    }
}