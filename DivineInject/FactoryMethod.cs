using System;
using System.Collections.Generic;
using System.Reflection;

namespace DivineInject
{
    public interface IFactoryMethod
    {
        ConstructorInfo Constructor { get; }
        IList<InjectableConstructorArg> Properties { get; }
        string Name { get; }
        Type ReturnType { get; }
        IList<IConstructorArg> ConstructorArgs { get; } 
    }

    internal class FactoryMethod : IFactoryMethod
    {
        public ConstructorInfo Constructor { get; private set; }
        public IList<InjectableConstructorArg> Properties { get; private set; }
        public string Name { get; private set; }
        public Type ReturnType { get; private set; }
        public IList<IConstructorArg> ConstructorArgs { get; private set; } 

        public FactoryMethod(ConstructorInfo constructor, IList<InjectableConstructorArg> properties, string name,
            Type returnType, IList<IConstructorArg> constructorArgs)
        {
            Constructor = constructor;
            Properties = properties;
            Name = name;
            ReturnType = returnType;
            ConstructorArgs = constructorArgs;
        }
    }
}
