using System.Collections.Generic;
using System.Reflection.Emit;

namespace DivineInject
{
    public interface IConstructorArgDefinition
    {
        IConstructorArg Define(TypeBuilder tb);
        IConstructorArg FindExisting(IList<IConstructorArg> arguments);
    }
}
