namespace DivineInject.Test.DummyModel
{
    public class DomainObjectWithOneDependency : IDomainObject
    {
        public DomainObjectWithOneDependency(IDatabase database)
        {
            Database = database;
        }

        public IDatabase Database { get; private set; }
    }
}