using System;

namespace DivineInject
{
    internal interface IPassedConstructorArg : IConstructorArg
    {
        Type ParameterType { get; }
        int ParameterIndex { get; }
    }

    internal class PassedConstructorArg : IPassedConstructorArg
    {
        public PassedConstructorArg(Type parameterType, int parameterIndex)
        {
            ParameterType = parameterType;
            ParameterIndex = parameterIndex;
        }

        public Type ParameterType { get; private set; }
        public int ParameterIndex { get; private set; }
    }
}
