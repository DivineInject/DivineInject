using DivineInject.Test.DummyModel;

namespace DivineInject.Test.FactoryGenerator
{
    internal interface IDummyFactory
    {
        IDomainObject MethodWithNoArgs();
        IDomainObject MethodWithSinglePassedArg(string name);
        IDomainObject MethodWithSingleDependency();
        IDomainObject MethodWithDependencyAndArg(string name);
        IDomainObject MethodWithDependencyAndTwoArgs(string name, int timeout);
        DomainObjectWithConstructorWithTwoArgsOfSameType MethodWithTwoArgsOfSameType(string role, string name);
    }
}
