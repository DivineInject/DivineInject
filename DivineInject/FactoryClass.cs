using System.Collections.Generic;

namespace DivineInject
{
    internal class FactoryClass
    {
        public IList<IFactoryMethod> Methods { get; private set; }

        public FactoryClass(IList<IFactoryMethod> methods)
        {
            Methods = methods;
        }
    }
}