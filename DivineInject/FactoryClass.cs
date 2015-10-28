using System.Collections.Generic;
using System.Linq;

namespace DivineInject
{
    internal class FactoryClass
    {
        public IList<IFactoryMethod> Methods { get; private set; }

        public FactoryClass(IConstructorArgList argList, IList<IFactoryMethod> methods)
        {
            Methods = methods;
            foreach (var definition in methods.SelectMany(m => m.ConstructorArgs))
                argList.Add(definition);
        }
    }
}