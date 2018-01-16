using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Instances;

namespace BlueMilk.IoC.Frames
{
    public class InjectedServiceField : InjectedField, IServiceVariable
    {
        public InjectedServiceField(Instance instance) : base(instance.ServiceType, DefaultArgName(instance.ServiceType) + instance.GetHashCode().ToString().Replace("-", "_"))
        {
            Instance = instance;
        }

        public Instance Instance { get; }

        public override string CtorArgDeclaration => $"[BlueMilk.Named(\"{Instance.Name}\")] {ArgType.NameInCode()} {CtorArg}";
    }
}