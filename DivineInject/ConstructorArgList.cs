using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace DivineInject
{
    internal class ConstructorArgList
    {
        public ConstructorArgList(TypeBuilder tb, params IConstructorArgDefinition[] definitions)
        {
            Arguments = new List<IConstructorArg>();
            foreach (var defn in definitions.Where(defn => defn.FindExisting(Arguments) == null))
                Arguments.Add(defn.Define(tb));
        }

        public IList<IConstructorArg> Arguments { get; private set; } 
    }
}
