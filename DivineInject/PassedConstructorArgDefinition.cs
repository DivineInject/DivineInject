using System;

namespace DivineInject
{
    class PassedConstructorArgDefinition : IConstructorArgDefinition
    {
        public PassedConstructorArgDefinition(Type parameterType)
        {
            ParameterType = parameterType;
        }

        public Type ParameterType { get; private set; }
    }
}
