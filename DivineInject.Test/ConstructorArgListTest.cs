using NUnit.Framework;
using TestFirst.Net.Extensions.Moq;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    [TestFixture]
    public class ConstructorArgListTest : AbstractNUnitMoqScenarioTest
    {
        [Test]
        public void EmptyList()
        {
            ConstructorArgList argList;
            Scenario()
                .Given(argList = new ConstructorArgList())

                .Then(argList.Arguments, Is(AList.NoItems<IConstructorArg>()));
        }
    }
}
