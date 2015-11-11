namespace DivineInject.Test.DummyModel
{
    public class DomainObjectWithConstructorWithTwoArgsOfSameType
    {
        public DomainObjectWithConstructorWithTwoArgsOfSameType(string name, string role)
        {
            Name = name;
            Role = role;
        }

        public string Name { get; private set; }
        public string Role { get; private set; }
    }
}
