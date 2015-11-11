namespace DivineInject.Test.DummyModel
{
    public class DomainObjectWithTwoConstructors : IDomainObjectWithName
    {
        public DomainObjectWithTwoConstructors()
        {
            Name = "Fred";
        }

        public DomainObjectWithTwoConstructors(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
