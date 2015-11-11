using DivineInject.Test.DummyModel;

namespace DivineInject.Test.FactoryGenerator
{
    internal interface IFactoryInterfaceWithTwoMethods
    {
        DomainObjectWithDependencyAndArg MethodWithDependencyOnly();
        DomainObjectWithDependencyAndArg MethodWithDependencyAndArg(string name);
    }
}
