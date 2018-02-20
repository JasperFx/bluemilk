using BlueMilk.IoC.Instances;

namespace BlueMilk
{
    /// <summary>
    /// Configures the BlueMilk instance name for resolving
    /// services by name
    /// </summary>
    public class InstanceNameAttribute : BlueMilkAttribute
    {
        private readonly string _name;

        public InstanceNameAttribute(string name)
        {
            _name = name;
        }

        public override void Alter(Instance instance)
        {
            instance.Name = _name;
        }
    }
}