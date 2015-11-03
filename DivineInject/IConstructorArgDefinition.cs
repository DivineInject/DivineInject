using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DivineInject
{
    public interface IConstructorArgDefinition
    {
        Type ParameterType { get; }

        IConstructorArg Define(TypeBuilder tb);
        IConstructorArg FindExisting(IList<IConstructorArg> arguments);
    }
}
