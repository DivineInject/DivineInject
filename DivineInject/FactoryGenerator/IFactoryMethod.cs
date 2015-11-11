using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace DivineInject.FactoryGenerator
{
    internal interface IFactoryMethod
    {
        ConstructorInfo Constructor { get; }
        string Name { get; }
        Type ReturnType { get; }
        Type ReturnImplType { get; }
        Type[] ParameterTypes { get; }
        IList<IConstructorArgDefinition> ConstructorArgs { get; }
        void EmitMethod(TypeBuilder tb, IConstructorArgList constructorArgList);
    }
}