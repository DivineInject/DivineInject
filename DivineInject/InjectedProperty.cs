using System;
using System.Reflection.Emit;

namespace DivineInject
{
    class InjectedProperty
    {
        public Type PropertyType { get; private set; }
        public string Name { get; private set; }

        internal MethodBuilder Getter { get; set; }
        internal MethodBuilder Setter { get; set; }

        public InjectedProperty(Type propertyType, string name)
        {
            PropertyType = propertyType;
            Name = name;
        }
    }
}
