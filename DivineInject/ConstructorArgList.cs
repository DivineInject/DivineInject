using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace DivineInject
{
    public interface IConstructorArgList
    {
        
    }

    internal class ConstructorArgList : IConstructorArgList
    {
        private readonly TypeBuilder m_tb;

        public ConstructorArgList(TypeBuilder tb, params IConstructorArgDefinition[] definitions)
        {
            m_tb = tb;
            Arguments = new List<IConstructorArg>();
            foreach (var defn in definitions)
                Add(defn);
        }

        public void Add(IConstructorArgDefinition defn)
        {
            if (defn.FindExisting(Arguments) == null)
                Arguments.Add(defn.Define(m_tb));
        }

        public IList<IConstructorArg> Arguments { get; private set; } 
    }
}
