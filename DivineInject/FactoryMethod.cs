using System.Collections.Generic;
using System.Reflection;

namespace DivineInject
{
    internal class FactoryMethod
    {
        public ConstructorInfo Constructor { get; private set; }
        public IList<InjectedDependencyProperty> Properties { get; private set; }

        public FactoryMethod(ConstructorInfo constructor, IList<InjectedDependencyProperty> properties)
        {
            Constructor = constructor;
            Properties = properties;
        }
    }
}
