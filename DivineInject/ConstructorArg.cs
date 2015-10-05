using System;

namespace DivineInject
{
    class ConstructorArg
    {
        public ConstructorArg(Type argType, int? propertyIndex, int? parameterIndex)
        {
            ArgType = argType;
            PropertyIndex = propertyIndex;
            ParameterIndex = parameterIndex;
        }

        public Type ArgType { get; private set; }
        public int? PropertyIndex { get; private set; }
        public int? ParameterIndex { get; private set; }
    }
}
