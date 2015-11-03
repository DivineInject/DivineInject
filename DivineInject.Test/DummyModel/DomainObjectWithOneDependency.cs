namespace DivineInject.Test.DummyModel
{
    public class DomainObjectWithOneDependency : IDomainObject
    {
        public IDatabase Database { get; set; }

        public DomainObjectWithOneDependency(IDatabase database)
        {
            Database = database;
        }
    }
}