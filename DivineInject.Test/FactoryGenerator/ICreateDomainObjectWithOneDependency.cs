using DivineInject.Test.DummyModel;

namespace DivineInject.Test.FactoryGenerator
{
    public interface ICreateDomainObjectWithOneDependency
    {
        IDomainObject Create();
    }
}