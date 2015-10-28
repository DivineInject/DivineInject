using NUnit.Framework;
using TestFirst.Net.Extensions.Moq;

namespace DivineInject.Test
{
    [TestFixture]
    public class FactoryClassTest : AbstractNUnitMoqScenarioTest
    {
        [Test]
        public void OnCreate_AddsAllConstructorArgDefinitionsToList()
        {
            IConstructorArgList argList;
            IFactoryMethod method1;
            IConstructorArgDefinition consArg1;
            Scenario()
                .Given(consArg1 = AMock<IConstructorArgDefinition>().Instance)
                .Given(method1 = AMock<IFactoryMethod>()
                    .WhereGet(m => m.ConstructorArgs).Returns(new [] { consArg1 })
                    .Instance)
                .Given(argList = AMock<IConstructorArgList>()
                    .WhereMethod(l => l.Add(consArg1))
                    .Instance)

                .When(new FactoryClass(argList, new[] {method1}))

                .Then(ExpectNoMocksFailed());
        }
    }
}
