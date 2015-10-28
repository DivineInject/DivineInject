using System.Collections.Generic;

namespace DivineInject
{
    internal class ConstructorArgList
    {
        public ConstructorArgList()
        {
            Arguments = new List<IConstructorArg>();
        }

        public IList<IConstructorArg> Arguments { get; private set; } 
    }
}
