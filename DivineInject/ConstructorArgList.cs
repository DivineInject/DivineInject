using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace DivineInject
{
    internal class ConstructorArgList
    {
        public ConstructorArgList(TypeBuilder tb, params IConstructorArgDefinition[] definitions)
        {
            Arguments = definitions.Select(d => d.Define(tb)).ToList();
        }

        public IList<IConstructorArg> Arguments { get; private set; } 
    }
}
