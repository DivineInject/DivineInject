using System.Collections.Generic;
using System.Reflection.Emit;

namespace DivineInject
{
    public interface IConstructorArgList
    {
        void Add(IConstructorArgDefinition defn);
        IList<IConstructorArg> Arguments { get; }
        IConstructorArg FindExisting(IConstructorArgDefinition defn);
    }

    internal class ConstructorArgList : IConstructorArgList
    {
        private readonly TypeBuilder m_tb;

        public ConstructorArgList(TypeBuilder tb)
        {
            m_tb = tb;
            Arguments = new List<IConstructorArg>();
        }

        public IList<IConstructorArg> Arguments { get; private set; } 

        public void Add(IConstructorArgDefinition defn)
        {
            if (FindExisting(defn) == null)
                Arguments.Add(defn.Define(m_tb));
        }

        public IConstructorArg FindExisting(IConstructorArgDefinition defn)
        {
            return defn.FindExisting(Arguments);
        }
    }
}
