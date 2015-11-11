using DivineInject.Test.DummyModel;

namespace DivineInject.Test.FactoryGenerator
{
    public interface ICreateDomainObjectWithDependencyAndArg
    {
        IDomainObjectWithName Create(string name);
    }
}