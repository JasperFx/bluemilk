namespace BlueMilk.IoC.Resolvers
{
    public class NoArgTransientResolver<T> : TransientResolver<T> where T : new()
    {
        public override object Build(Scope scope)
        {
            return new T();
        }
    }
}