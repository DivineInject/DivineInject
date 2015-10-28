namespace DivineInject.Test.DummyModel
{
    class DomainObjectWithDependencyAndTwoArgs : IDomainObject
    {
        public DomainObjectWithDependencyAndTwoArgs(IDatabase database, string name, int timeout)
        {
        }
    }
}
