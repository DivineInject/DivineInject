using System;
using TestFirst.Net;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    internal class APassedConstructorArg : PropertyMatcher<PassedConstructorArg>
    {
        private static readonly PassedConstructorArg PropertyNames = null;

        private APassedConstructorArg()
        {
        }

        public static APassedConstructorArg With()
        {
            return new APassedConstructorArg();
        }

        public APassedConstructorArg Type(Type type)
        {
            return Type(AnInstance.SameAs(type));
        }

        public APassedConstructorArg Type(IMatcher<Type> type)
        {
            WithProperty(() => PropertyNames.ParameterType, type);
            return this;
        }
    }
}
