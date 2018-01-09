namespace BlueMilk.IoC.Resolvers
{
    public class NoArgSingletonResolver<T> : SingletonResolver<T> where T : class, new()
    {
        public NoArgSingletonResolver(Scope topLevelScope) : base(topLevelScope)
        {
        }

        public override T Build(Scope scope)
        {
            return new T();
        }
    }
}