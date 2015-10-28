namespace DivineInject.Test.DummyModel
{
    internal class DomainObjectWithOneDependency : IDomainObject
    {
        public DomainObjectWithOneDependency(IDatabase database)
        {
        }
    }
}