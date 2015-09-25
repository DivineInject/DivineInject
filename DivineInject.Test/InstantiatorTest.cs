using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestFirst.Net.Extensions.Moq;
using TestFirst.Net.Matcher;
using DivineInject;

namespace DivineInject.Test
{
    [TestFixture]
    public class InstantiatorTest : AbstractNUnitMoqScenarioTest
    {
        [Test]
        public void CreatesInstanceUsingNoArgConstructor()
        {
            Instantiator instantiator;
            object instance;

            Scenario()
                .Given(instantiator = new Instantiator(AMock<IDivineInjector>().Instance))

                .When(instance = instantiator.Create<TestClassWithNoArgConstructor>())

                .Then((TestClassWithNoArgConstructor) instance, Is(AnInstance.NotNull<TestClassWithNoArgConstructor>()))
            ;
        }

        [Test]
        public void CreatesInstanceResolvingConstructorParamsViaInjector()
        {
            Instantiator instantiator;
            TestClassWithConstructorWithDependency instance;
            ITestDependency dependency;
            IDivineInjector injector;

            Scenario()
                .Given(dependency = AMock<ITestDependency>().Instance)
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.Get(typeof(ITestDependency))).Returns(dependency)
                    .Instance)
                .Given(instantiator = new Instantiator(injector))

                .When(instance = instantiator.Create<TestClassWithConstructorWithDependency>())

                .Then(instance, Is(AnInstance.NotNull<TestClassWithConstructorWithDependency>()))
                .Then(instance.Dependency, Is(AnInstance.SameAs(dependency)))
            ;
        }

    }

    public interface ITestDependency
    { }

    public class TestClassWithNoArgConstructor
    { }

    public class TestClassWithConstructorWithDependency
    {
        public TestClassWithConstructorWithDependency(ITestDependency dependency)
        {
            Dependency = dependency;
        }

        public ITestDependency Dependency { get; private set; }
    }
}
