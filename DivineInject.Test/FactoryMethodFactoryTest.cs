using System;
using System.Reflection;
using NUnit.Framework;
using TestFirst.Net.Extensions.Moq;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    [TestFixture]
    public class FactoryMethodFactoryTest : AbstractNUnitMoqScenarioTest
    {
        [Test]
        public void CreatesFactoryMethodForMethodWithNoArgs()
        {
            FactoryMethodFactory factoryMethodFactory;
            MethodInfo methodInfo;
            IDivineInjector injector;
            FactoryMethod factoryMethod;
            Type domainObjectType;

            Scenario()
                .Given(factoryMethodFactory = new FactoryMethodFactory())
                .Given(methodInfo = typeof (IDummyFactory).GetMethod("MethodWithNoArgs"))
                .Given(injector = AMock<IDivineInjector>().Instance)
                .Given(domainObjectType = typeof(DomainObjectWithDefaultConstructor))

                .When(factoryMethod = factoryMethodFactory.Create(methodInfo, injector, domainObjectType))

                .Then(factoryMethod.Constructor, Is(AnInstance.SameAs(domainObjectType.GetConstructor(new Type[0]))))
                .Then(factoryMethod.Properties, Is(AList.NoItems<GeneratedProperty>()))
            ;
        }
    }

    internal interface IDummyFactory
    {
        string MethodWithNoArgs();
    }

    internal class DomainObjectWithDefaultConstructor
    {
    }
}
