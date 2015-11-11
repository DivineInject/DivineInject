namespace DivineInject.Test.DummyModel
{
    public class DomainObjectWithSingleArgConstructor : IDomainObjectWithName
    {
        public DomainObjectWithSingleArgConstructor(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}