namespace DivineInject.Test.DummyModel
{
    public class DomainObjectWithDependencyAndArg : IDomainObject
    {
        public IDatabase Database { get; set; }
        public string Name { get; set; }

        public DomainObjectWithDependencyAndArg(IDatabase database, string name)
        {
            Database = database;
            Name = name;
        }
    }
}