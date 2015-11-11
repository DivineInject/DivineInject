using System;
using System.Reflection.Emit;

namespace DivineInject.FactoryGenerator
{
    internal interface IInjectableConstructorArg : IConstructorArg
    {
        Type PropertyType { get; }
        string Name { get; }
        MethodBuilder Getter { get; }
        MethodBuilder Setter { get; }
    }
}