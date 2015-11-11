using DivineInject.Test.DummyModel;

namespace DivineInject.Test.FactoryGenerator
{
    public interface ICreateDomainObjectWithSingleArgConstructor
    {
        IDomainObjectWithName Create(string name);
    }
}