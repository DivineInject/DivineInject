namespace DivineInject.Test.DummyModel
{
    public class DomainObjectWithSingleArgConstructor : IDomainObjectWithName
    {
        public string Name { get; private set; }

        public DomainObjectWithSingleArgConstructor(string name)
        {
            Name = name;
        }
    }
}