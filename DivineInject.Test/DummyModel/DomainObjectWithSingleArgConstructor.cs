namespace DivineInject.Test.DummyModel
{
    public class DomainObjectWithSingleArgConstructor : IDomainObject
    {
        public string Name { get; set; }

        public DomainObjectWithSingleArgConstructor(string name)
        {
            Name = name;
        }
    }
}