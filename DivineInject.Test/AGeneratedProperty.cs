using System;
using TestFirst.Net;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    internal class AGeneratedProperty : PropertyMatcher<GeneratedProperty>
    {
        private static readonly GeneratedProperty PropertyNames = null;

        private AGeneratedProperty()
        {
        }

        public static AGeneratedProperty With()
        {
            return new AGeneratedProperty();
        }

        public AGeneratedProperty PropertyType(Type propertyType)
        {
            return PropertyType(AnInstance.SameAs(propertyType));
        }

        public AGeneratedProperty PropertyType(IMatcher<Type> propertyType)
        {
            WithProperty(() => PropertyNames.PropertyType, propertyType);
            return this;
        }

        public AGeneratedProperty Name(string name)
        {
            return Name(AString.EqualTo(name));
        }

        public AGeneratedProperty Name(IMatcher<string> name)
        {
            WithProperty(() => PropertyNames.Name, name);
            return this;
        }

        public AGeneratedProperty PropertyValue(object propertyValue)
        {
            return PropertyValue(AnInstance.SameAs(propertyValue));
        }

        public AGeneratedProperty PropertyValue(IMatcher<object> propertyValue)
        {
            WithProperty(() => PropertyNames.PropertyValue, propertyValue);
            return this;
        }
    }
}