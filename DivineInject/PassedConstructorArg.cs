using System;

namespace DivineInject
{
    class PassedConstructorArg : IConstructorArg
    {
        public PassedConstructorArg(Type parameterType)
        {
            ParameterType = parameterType;
        }

        public Type ParameterType { get; private set; }
    }
}
