using System;
using System.Collections.Generic;
using System.Reflection;

namespace DivineInject
{
    public interface IFactoryMethod
    {
        ConstructorInfo Constructor { get; }
        string Name { get; }
        Type ReturnType { get; }
        Type ReturnImplType { get; }
        Type[] ParameterTypes { get; }
        IList<IConstructorArgDefinition> ConstructorArgs { get; } 
    }

    internal class FactoryMethod : IFactoryMethod
    {
        public ConstructorInfo Constructor { get; private set; }
        public string Name { get; private set; }
        public Type ReturnType { get; private set; }
        public Type ReturnImplType { get; private set; }
        public Type[] ParameterTypes { get; private set; }
        public IList<IConstructorArgDefinition> ConstructorArgs { get; private set; } 

        public FactoryMethod(ConstructorInfo constructor, string name,
            Type returnType, Type returnImplType, Type[] parameterTypes, IList<IConstructorArgDefinition> constructorArgs)
        {
            Constructor = constructor;
            Name = name;
            ReturnType = returnType;
            ReturnImplType = returnImplType;
            ParameterTypes = parameterTypes;
            ConstructorArgs = constructorArgs;
        }
    }
}
