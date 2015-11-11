using DivineInject.Test.DummyModel;

namespace DivineInject.Test.FactoryGenerator
{
    public interface ICreateDomainObjectWithConstructorWithTwoArgsOfSameType
    {
        DomainObjectWithConstructorWithTwoArgsOfSameType Create(string role, string name);
    }
}