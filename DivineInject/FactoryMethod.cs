using System.Collections.Generic;
using System.Reflection;

namespace DivineInject
{
    internal class FactoryMethod
    {
        public ConstructorInfo Constructor { get; private set; }
        public IList<InjectedProperty> Properties { get; private set; }

        public FactoryMethod(ConstructorInfo constructor, IList<InjectedProperty> properties)
        {
            Constructor = constructor;
            Properties = properties;
        }
    }
}
