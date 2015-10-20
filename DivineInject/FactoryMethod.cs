using System;
using System.Collections.Generic;
using System.Reflection;

namespace DivineInject
{
    public interface IFactoryMethod
    {
        ConstructorInfo Constructor { get; }
        IList<InjectedProperty> Properties { get; }
        string Name { get; }
        Type ReturnType { get; }
    }

    internal class FactoryMethod : IFactoryMethod
    {
        public ConstructorInfo Constructor { get; private set; }
        public IList<InjectedProperty> Properties { get; private set; }
        public string Name { get; private set; }
        public Type ReturnType { get; private set; }

        public FactoryMethod(ConstructorInfo constructor, IList<InjectedProperty> properties, string name,
            Type returnType)
        {
            Constructor = constructor;
            Properties = properties;
            Name = name;
            ReturnType = returnType;
        }
    }
}
