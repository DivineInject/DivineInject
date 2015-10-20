using System;

namespace DivineInject
{
    public class InjectableConstructorArgDefinition : IConstructorArgDefinition
    {
        public Type PropertyType { get; private set; }
        public string Name { get; private set; }

        public InjectableConstructorArgDefinition(Type propertyType, string name)
        {
            PropertyType = propertyType;
            Name = name;
        }
    }
}
