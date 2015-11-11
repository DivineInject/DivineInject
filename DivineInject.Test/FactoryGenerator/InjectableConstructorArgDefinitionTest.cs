using System.Collections.Generic;
using DivineInject.FactoryGenerator;
using DivineInject.Test.DummyModel;
using NUnit.Framework;
using TestFirst.Net.Extensions.Moq;
using TestFirst.Net.Matcher;

namespace DivineInject.Test.FactoryGenerator
{
    [TestFixture]
    public class InjectableConstructorArgDefinitionTest : AbstractNUnitMoqScenarioTest
    {
        [Test]
        public void FindsMatchingConstructorArg()
        {
            InjectableConstructorArgDefinition definition;
            IInjectableConstructorArg arg1;
            IConstructorArg resut;

            Scenario()
                .Given(arg1 = AMock<IInjectableConstructorArg>()
                    .WhereGet(a => a.Name).Returns("database")
                    .WhereGet(a => a.PropertyType).Returns(typeof(IDatabase))
                    .Instance)

                .Given(definition = new InjectableConstructorArgDefinition(typeof(IDatabase), "database"))

                .When(resut = definition.FindExisting(new[] { arg1 }))

                .Then(resut, Is(AnInstance.SameAs((IConstructorArg)arg1)));
        }

        [Test]
        public void FindExisting_ReturnsNullInCaseNotAlreadyDefined()
        {
            InjectableConstructorArgDefinition definition;
            IConstructorArg resut;

            Scenario()
                .Given(definition = new InjectableConstructorArgDefinition(typeof(IDatabase), "database"))

                .When(resut = definition.FindExisting(new List<IConstructorArg>()))

                .Then(resut, Is(AnInstance.Null<IConstructorArg>()));
        }
    }
}
