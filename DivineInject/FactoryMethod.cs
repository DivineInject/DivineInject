using System.Collections.Generic;
using System.Reflection;

namespace DivineInject
{
    public interface IFactoryMethod
    {
        ConstructorInfo Constructor { get; }
        IList<InjectedProperty> Properties { get; }
    }

    internal class FactoryMethod : IFactoryMethod
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
