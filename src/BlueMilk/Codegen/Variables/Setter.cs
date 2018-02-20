using System;
using BlueMilk.Compilation;

namespace BlueMilk.Codegen.Variables
{
    public class Setter : Variable
    {
        public Setter(Type variableType) : base(variableType)
        {
        }

        public Setter(Type variableType, string name) : base(variableType, name)
        {
        }

        public void WriteDeclaration(ISourceWriter writer)
        {
            writer.Write($"public {VariableType.FullNameInCode()} {Usage} {{get; set;}}");
        }
        
        /// <summary>
        /// Value to be set upon creating an instance of the class
        /// </summary>
        public object InitialValue { get; set; }

        public void SetInitialValue(object @object)
        {
            var property = @object.GetType().GetProperty(Usage);
            property.SetValue(@object, InitialValue);
        }
    }
}