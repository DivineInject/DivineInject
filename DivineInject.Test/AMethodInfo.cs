using System.Reflection;
using TestFirst.Net;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    public class AMethodInfo : PropertyMatcher<MethodInfo>
    {
        private static readonly MethodInfo PropertyNames = null;

        private AMethodInfo()
        {
        }

        public static AMethodInfo With()
        {
            return new AMethodInfo();
        }

        public AMethodInfo Name(string name)
        {
            return Name(AString.EqualTo(name));
        }

        public AMethodInfo Name(IMatcher<string> name)
        {
            WithProperty(() => PropertyNames.Name, name);
            return this;
        }
    }
}