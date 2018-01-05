using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Baseline;
using BlueMilk.Codegen.Methods;
using BlueMilk.Codegen.Variables;
using BlueMilk.Compilation;

namespace BlueMilk.Codegen
{
    public class GeneratedType
    {
        public GenerationRules Rules { get; }

        public string TypeName { get; }
        private Type _baseType;
        private readonly IList<Type> _interfaces = new List<Type>();
        private readonly IList<IGeneratedMethod> _methods = new List<IGeneratedMethod>();

        public GeneratedType(GenerationRules rules, string typeName)
        {
            Rules = rules;
            TypeName = typeName;
        }

        public Visibility Visibility { get; set; } = Visibility.Public;

        public Type BaseType
        {
            get => _baseType;
            set
            {
                if (value == null)
                {
                    _baseType = null;
                    return;
                }

                _baseType = value;
            }
        }

        // TODO -- need ut's
        public void AddInterface(Type type)
        {
            if (!type.GetTypeInfo().IsInterface)
            {
                throw new ArgumentOutOfRangeException(nameof(type), "Must be an interface type");
            }

            _interfaces.Add(type);
        }

        // TODO -- need ut's
        public void AddInterface<T>()
        {
            AddInterface(typeof(T));
        }

        public IEnumerable<Type> Interfaces => _interfaces;


        public IEnumerable<IGeneratedMethod> Methods => _methods;

        // TODO -- need ut's
        public void AddMethod(IGeneratedMethod method)
        {
            _methods.Add(method);
        }

        public SyncVoidGeneratedMethod AddVoidMethod(string name, params Argument[] args)
        {
            var method = new SyncVoidGeneratedMethod(name, args);
            AddMethod(method);

            return method;
        }

        public SyncSingleReturnGeneratedMethod AddSyncMethodThatReturns<TReturn>(string name, params Argument[] args)
        {
            var method = new SyncSingleReturnGeneratedMethod(name, typeof(TReturn), args);
            AddMethod(method);

            return method;
        }
        
        public string SourceCode { get; set; }


        public void Write(ISourceWriter writer)
        {

            writeDeclaration(writer);

            var args = Args();
            writeFieldDeclarations(writer, args);
            writeConstructorMethod(writer, args);


            foreach (var method in _methods)
            {
                writer.BlankLine();
                method.WriteMethod(writer);
            }

            writer.FinishBlock();
        }

        public InjectedField[] Args()
        {
            var args = _methods.SelectMany(x => x.Fields).Distinct().ToArray();
            return args;
        }

        private void writeConstructorMethod(ISourceWriter writer, InjectedField[] args)
        {
            var ctorArgs = args.Select(x => x.CtorArgDeclaration).Join(", ");
            writer.Write($"BLOCK:public {TypeName}({ctorArgs})");

            foreach (var field in args)
            {
                field.WriteAssignment(writer);
            }

            writer.FinishBlock();
        }

        private void writeFieldDeclarations(ISourceWriter writer, InjectedField[] args)
        {
            foreach (var field in args)
            {
                field.WriteDeclaration(writer);
            }

            writer.BlankLine();
        }

        private void writeDeclaration(ISourceWriter writer)
        {
            var implemented = implements().ToArray();

            if (implemented.Any())
            {
                writer.Write($"BLOCK:public class {TypeName} : {implemented.Select(x => x.FullName).Join(", ")}");
            }
            else
            {
                writer.Write($"BLOCK:public class {TypeName}");
            }
        }

        private IEnumerable<Type> implements()
        {
            if (_baseType != null)
            {
                yield return _baseType;
            }

            foreach (var @interface in Interfaces)
            {
                yield return @interface;
            }
        }
        
        public Type CompiledType { get; private set; }

        public void FindType(Type[] generated)
        {
            CompiledType = generated.Single(x => x.Name == TypeName);
        }

        public void ArrangeFrames()
        {
            foreach (var method in _methods)
            {
                method.ArrangeFrames(this);
            }
        }

        public IEnumerable<Assembly> AssemblyReferences()
        {
            if (_baseType != null) yield return _baseType.Assembly;

            foreach (var @interface in _interfaces)
            {
                yield return @interface.Assembly;
            }
        }

        public T CreateInstance<T>(params object[] arguments)
        {
            if (CompiledType == null)
            {
                throw new InvalidOperationException("This generated assembly has not yet been successfully compiled");
            }

            return (T) Activator.CreateInstance(CompiledType, arguments);
        }
    }
}
