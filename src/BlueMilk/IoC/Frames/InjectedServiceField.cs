using BlueMilk.Codegen;
using BlueMilk.Codegen.Variables;
using BlueMilk.IoC.Instances;
using BlueMilk.Scanning.Conventions;

namespace BlueMilk.IoC.Frames
{
    public class InjectedServiceField : InjectedField, IServiceVariable
    {
        private bool _isOnlyOne;

        public InjectedServiceField(Instance instance) : base(instance.ServiceType.MustBeBuiltWithFunc() ? typeof(object) : instance.ServiceType,
            instance.DefaultArgName())
        {
            Instance = instance;
            _isOnlyOne = instance.IsOnlyOneOfServiceType;
        }

        public bool IsOnlyOne
        {
            private get => _isOnlyOne;
            set
            {
                _isOnlyOne = value;
                if (value)
                {
                    var defaultArgName = DefaultArgName(VariableType);
                    OverrideName("_" +defaultArgName);
                    CtorArg = defaultArgName;
                }
            }
        }

        public override string CtorArgDeclaration
        {
            get
            {
                if (Instance.ServiceType.MustBeBuiltWithFunc())
                {
                    return $"[BlueMilk.Named(\"{Instance.Name}\", \"{Instance.ServiceType.FullNameInCode()}\")] object {CtorArg}";
                }

                return IsOnlyOne
                    ? $"{ArgType.NameInCode()} {CtorArg}"
                    : $"[BlueMilk.Named(\"{Instance.Name}\")] {ArgType.NameInCode()} {CtorArg}";
            }
        }

        public Instance Instance { get; }
    }
}
