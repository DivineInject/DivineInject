namespace DivineInject.Test.DummyModel
{
    public class DomainObjectWithDependencyAndArg : IDomainObjectWithName
    {
        public DomainObjectWithDependencyAndArg(IDatabase database, string name)
        {
            Database = database;
            Name = name;
        }

        public IDatabase Database { get; private set; }
        public string Name { get; private set; }
    }
}