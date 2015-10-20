using System;
using System.Reflection.Emit;

namespace DivineInject
{
    public class InjectableConstructorArgDefinition : IConstructorArgDefinition
    {
        public Type PropertyType { get; private set; }
        public string Name { get; private set; }

        internal MethodBuilder Getter { get; set; }
        internal MethodBuilder Setter { get; set; }

        public InjectableConstructorArgDefinition(Type propertyType, string name)
        {
            PropertyType = propertyType;
            Name = name;
        }
    }
}
