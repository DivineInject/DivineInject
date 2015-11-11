namespace DivineInject.Test.DummyModel
{
    internal class DomainObjectWithDependencyAndTwoArgs : IDomainObject
    {
        public DomainObjectWithDependencyAndTwoArgs(IDatabase database, string name, int timeout)
        {
        }
    }
}
