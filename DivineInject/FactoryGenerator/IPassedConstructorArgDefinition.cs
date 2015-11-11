namespace DivineInject.FactoryGenerator
{
    internal interface IPassedConstructorArgDefinition : IConstructorArgDefinition
    {
        int ParameterIndex { get; }
    }
}