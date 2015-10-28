namespace DivineInject.Test.DummyModel
{
    class DomainObjectWithDependencyAndTwoArgs
    {
        public DomainObjectWithDependencyAndTwoArgs(IDatabase database, string name, int timeout)
        {
        }
    }
}
