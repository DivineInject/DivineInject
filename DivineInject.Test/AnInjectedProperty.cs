using System;
using TestFirst.Net;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    internal class AnInjectedProperty : PropertyMatcher<InjectedProperty>
    {
        private static readonly InjectedProperty PropertyNames = null;

        private AnInjectedProperty()
        {
        }

        public static AnInjectedProperty With()
        {
            return new AnInjectedProperty();
        }

        public AnInjectedProperty PropertyType(Type propertyType)
        {
            return PropertyType(AnInstance.SameAs(propertyType));
        }

        public AnInjectedProperty PropertyType(IMatcher<Type> propertyType)
        {
            WithProperty(() => PropertyNames.PropertyType, propertyType);
            return this;
        }

        public AnInjectedProperty Name(string name)
        {
            return Name(AString.EqualTo(name));
        }

        public AnInjectedProperty Name(IMatcher<string> name)
        {
            WithProperty(() => PropertyNames.Name, name);
            return this;
        }
    }
}