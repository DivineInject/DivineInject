using System;
using System.Reflection.Emit;

namespace DivineInject
{
    class InjectedDependencyProperty
    {
        public Type PropertyType { get; private set; }
        public string Name { get; private set; }
        public object PropertyValue { get; private set; }

        internal MethodBuilder Getter { get; set; }
        internal MethodBuilder Setter { get; set; }

        public InjectedDependencyProperty(Type propertyType, string name, object value)
        {
            PropertyType = propertyType;
            Name = name;
            PropertyValue = value;
        }
    }
}
