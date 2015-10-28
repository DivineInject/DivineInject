using System.Collections.Generic;

namespace DivineInject
{
    internal class FactoryClass
    {
        public IList<IFactoryMethod> Methods { get; private set; }

        public FactoryClass(IConstructorArgList argList, IList<IFactoryMethod> methods)
        {
            Methods = methods;
        }
    }
}