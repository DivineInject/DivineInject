using System;

namespace DivineInject
{
    class PassedConstructorArgDefinition : IConstructorArgDefinition
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
    }
}
