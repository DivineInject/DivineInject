namespace DivineInject.FactoryGenerator
{
    internal interface IInjectableConstructorArgDefinition : IConstructorArgDefinition
    {
        string Name { get; }
    }
}