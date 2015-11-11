using System;
using TestFirst.Net.Matcher;

namespace DivineInject.Test.Matchers
{
    public class AType : PropertyMatcher<Type>
    {
        private AType(Type expectedType)
        {
            WithMatcher("type fullname", t => t.FullName, AString.EqualTo(expectedType.FullName));
        }

        public static AType EqualTo(Type type)
        {
            return new AType(type);
        }
    }
}