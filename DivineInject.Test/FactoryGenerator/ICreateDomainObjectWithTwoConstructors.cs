using DivineInject.Test.DummyModel;

namespace DivineInject.Test.FactoryGenerator
{
    public interface ICreateDomainObjectWithTwoConstructors
    {
        IDomainObjectWithName CreateWithDefaultName();
        IDomainObjectWithName CreateWithName(string name);
    }
}