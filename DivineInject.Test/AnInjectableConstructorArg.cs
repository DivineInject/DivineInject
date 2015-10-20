using System;
using TestFirst.Net;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    internal class AnInjectableConstructorArg : PropertyMatcher<InjectableConstructorArg>
    {
        private static readonly InjectableConstructorArg PropertyNames = null;

        private AnInjectableConstructorArg()
        {
        }

        public static AnInjectableConstructorArg With()
        {
            return new AnInjectableConstructorArg();
        }

        public AnInjectableConstructorArg PropertyType(Type propertyType)
        {
            return PropertyType(AnInstance.SameAs(propertyType));
        }

        public AnInjectableConstructorArg PropertyType(IMatcher<Type> propertyType)
        {
            WithProperty(() => PropertyNames.PropertyType, propertyType);
            return this;
        }

        public AnInjectableConstructorArg Name(string name)
        {
            return Name(AString.EqualTo(name));
        }

        public AnInjectableConstructorArg Name(IMatcher<string> name)
        {
            WithProperty(() => PropertyNames.Name, name);
            return this;
        }
    }
}