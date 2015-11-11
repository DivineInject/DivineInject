using System;
using DivineInject.FactoryGenerator;
using TestFirst.Net;
using TestFirst.Net.Matcher;

namespace DivineInject.Test.Matchers
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

        public APassedConstructorArgDefinition Index(int index)
        {
            return Index(AnInt.EqualTo(index));
        }

        public APassedConstructorArgDefinition Index(IMatcher<int?> index)
        {
            WithProperty(() => PropertyNames.ParameterIndex, index);
            return this;
        }
    }
}
