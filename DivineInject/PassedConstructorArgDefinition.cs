using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DivineInject
{
    public interface IPassedConstructorArgDefinition : IConstructorArgDefinition
    {
        int ParameterIndex { get; }
    }

    internal class PassedConstructorArgDefinition : IPassedConstructorArgDefinition
    {
        public PassedConstructorArgDefinition(Type parameterType, int parameterIndex)
        {
            ParameterType = parameterType;
            ParameterIndex = parameterIndex;
        }

        public Type ParameterType { get; private set; }
        public int ParameterIndex { get; private set; }

        public override string ToString()
        {
            return string.Format("PassedConstructorArg(Type={0}, Index={1})", ParameterType.FullName, ParameterIndex);
        }

        public IConstructorArg Define(TypeBuilder tb)
        {
            return new PassedConstructorArg(ParameterType, ParameterIndex);
        }

        public IConstructorArg FindExisting(IList<IConstructorArg> arguments)
        {
            return null;
        }
    }
}
