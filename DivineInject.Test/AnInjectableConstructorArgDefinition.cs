using System;
using TestFirst.Net;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    internal class AnInjectableConstructorArgDefinition : PropertyMatcher<InjectableConstructorArgDefinition>
    {
        private static readonly InjectableConstructorArgDefinition PropertyNames = null;

        private AnInjectableConstructorArgDefinition()
        {
        }

        public static AnInjectableConstructorArgDefinition With()
        {
            return new AnInjectableConstructorArgDefinition();
        }

        public AnInjectableConstructorArgDefinition PropertyType(Type propertyType)
        {
            return PropertyType(AnInstance.SameAs(propertyType));
        }

        public AnInjectableConstructorArgDefinition PropertyType(IMatcher<Type> propertyType)
        {
            WithProperty(() => PropertyNames.ParameterType, propertyType);
            return this;
        }

        public AnInjectableConstructorArgDefinition Name(string name)
        {
            return Name(AString.EqualTo(name));
        }

        public AnInjectableConstructorArgDefinition Name(IMatcher<string> name)
        {
            WithProperty(() => PropertyNames.Name, name);
            return this;
        }
    }
}