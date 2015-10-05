using System;

namespace DivineInject
{
    class ConstructorArg
    {
        public ConstructorArg(Type argType, int propertyIndex)
        {
            ArgType = argType;
            PropertyIndex = propertyIndex;
        }

        public Type ArgType { get; private set; }
        public int PropertyIndex { get; private set; }
    }
}
