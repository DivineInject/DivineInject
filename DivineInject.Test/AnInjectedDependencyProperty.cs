using System;
using TestFirst.Net;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    internal class AnInjectedDependencyProperty : PropertyMatcher<InjectedDependencyProperty>
    {
        private static readonly InjectedDependencyProperty PropertyNames = null;

        private AnInjectedDependencyProperty()
        {
        }

        public static AnInjectedDependencyProperty With()
        {
            return new AnInjectedDependencyProperty();
        }

        public AnInjectedDependencyProperty PropertyType(Type propertyType)
        {
            return PropertyType(AnInstance.SameAs(propertyType));
        }

        public AnInjectedDependencyProperty PropertyType(IMatcher<Type> propertyType)
        {
            WithProperty(() => PropertyNames.PropertyType, propertyType);
            return this;
        }

        public AnInjectedDependencyProperty Name(string name)
        {
            return Name(AString.EqualTo(name));
        }

        public AnInjectedDependencyProperty Name(IMatcher<string> name)
        {
            WithProperty(() => PropertyNames.Name, name);
            return this;
        }
    }
}