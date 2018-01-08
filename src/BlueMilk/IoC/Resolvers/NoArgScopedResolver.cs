namespace BlueMilk.IoC.Resolvers
{
    public class NoArgScopedResolver<T> : ScopedResolver<T> where T : new()
    {
        public override object Build(Scope scope)
        {
            return new T();
        }
    }
}