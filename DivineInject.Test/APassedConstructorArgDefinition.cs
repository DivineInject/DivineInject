using System;
using TestFirst.Net;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    internal class APassedConstructorArgDefinition : PropertyMatcher<PassedConstructorArgDefinition>
    {
        private static readonly PassedConstructorArgDefinition PropertyNames = null;

        private APassedConstructorArgDefinition()
        {
        }

        public static APassedConstructorArgDefinition With()
        {
            return new APassedConstructorArgDefinition();
        }

        public APassedConstructorArgDefinition Type(Type type)
        {
            return Type(AnInstance.SameAs(type));
        }

        public APassedConstructorArgDefinition Type(IMatcher<Type> type)
        {
            WithProperty(() => PropertyNames.ParameterType, type);
            return this;
        }
    }
}
