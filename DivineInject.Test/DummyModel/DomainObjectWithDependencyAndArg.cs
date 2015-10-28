namespace DivineInject.Test.DummyModel
{
    internal class DomainObjectWithDependencyAndArg : IDomainObject
    {
        public DomainObjectWithDependencyAndArg(IDatabase database, string name)
        {
        }
    }
}