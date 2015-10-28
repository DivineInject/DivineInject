﻿using System;

namespace DivineInject
{
    class PassedConstructorArg : IConstructorArg
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
