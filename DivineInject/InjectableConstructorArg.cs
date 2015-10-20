using System;
using System.Reflection.Emit;

namespace DivineInject
{
    public class InjectableConstructorArg
    {
        public Type PropertyType { get; private set; }
        public string Name { get; private set; }

        public MethodBuilder Getter { get; private set; }
        public MethodBuilder Setter { get; private set; }

        public InjectableConstructorArg(Type propertyType, string name, MethodBuilder getter, MethodBuilder setter)
        {
            PropertyType = propertyType;
            Name = name;
            Getter = getter;
            Setter = setter;
        }
    }
}