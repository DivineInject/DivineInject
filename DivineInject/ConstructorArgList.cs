using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace DivineInject
{
    public interface IConstructorArgList
    {
        void Add(IConstructorArgDefinition defn);
        IList<IConstructorArg> Arguments { get; }
    }

    internal class ConstructorArgList : IConstructorArgList
    {
        private readonly TypeBuilder m_tb;

        public ConstructorArgList(TypeBuilder tb)
        {
            m_tb = tb;
            Arguments = new List<IConstructorArg>();
        }

        public void Add(IConstructorArgDefinition defn)
        {
            if (defn.FindExisting(Arguments) == null)
                Arguments.Add(defn.Define(m_tb));
        }

        public IList<IConstructorArg> Arguments { get; private set; } 
    }
}
