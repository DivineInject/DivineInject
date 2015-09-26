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
                    .WhereMethod(i => i.IsBound(typeof(ITestDependency))).Returns(true)
                    .WhereMethod(i => i.Get(typeof(ITestDependency))).Returns(dependency)
                    .Instance)
                .Given(instantiator = new Instantiator(injector))

                .When(instance = instantiator.Create<TestClassWithConstructorWithDependency>())

                .Then(instance, Is(AnInstance.NotNull<TestClassWithConstructorWithDependency>()))
                .Then(instance.Dependency, Is(AnInstance.SameAs(dependency)))
            ;
        }

        [Test]
        public void CreatesInstanceUsingConstructorWithFewestNumberOfParameters()
        {
            Instantiator instantiator;
            TestClassWithTwoConstructorsWithDependencies instance;
            ITestDependency dependency;
            IDivineInjector injector;

            Scenario()
                .Given(dependency = AMock<ITestDependency>().Instance)
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(ITestDependency))).Returns(true)
                    .WhereMethod(i => i.IsBound(typeof(ITestSecondDependency))).Returns(false)
                    .WhereMethod(i => i.Get(typeof(ITestDependency))).Returns(dependency)
                    .Instance)
                .Given(instantiator = new Instantiator(injector))

                .When(instance = instantiator.Create<TestClassWithTwoConstructorsWithDependencies>())

                .Then(instance, Is(AnInstance.NotNull<TestClassWithTwoConstructorsWithDependencies>()))
                .Then(instance.Dependency, Is(AnInstance.SameAs(dependency)))
            ;
        }

        [Test]
        public void CreatesInstanceUsingFirstConstructorThatCanBeInjected()
        {
            Instantiator instantiator;
            TestClassWithAnUninjectableConstructor instance;
            ITestDependency dependency;
            IDivineInjector injector;

            Scenario()
                .Given(dependency = AMock<ITestDependency>().Instance)
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(ITestDependency))).Returns(true)
                    .WhereMethod(i => i.IsBound(typeof(string))).Returns(false)
                    .WhereMethod(i => i.Get(typeof(ITestDependency))).Returns(dependency)
                    .Instance)
                .Given(instantiator = new Instantiator(injector))

                .When(instance = instantiator.Create<TestClassWithAnUninjectableConstructor>())

                .Then(instance, Is(AnInstance.NotNull<TestClassWithAnUninjectableConstructor>()))
                .Then(instance.Dependency, Is(AnInstance.SameAs(dependency)))
            ;
        }

        [Test]
        public void CreateThrowsExceptionInCaseNoInjectableConstructorsFound()
        {
            Instantiator instantiator;
            ITestDependency dependency;
            IDivineInjector injector;
            Exception exception;

            Scenario()
                .Given(dependency = AMock<ITestDependency>().Instance)
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(ITestSecondDependency))).Returns(false)
                    .WhereMethod(i => i.IsBound(typeof(string))).Returns(false)
                    .Instance)
                .Given(instantiator = new Instantiator(injector))

                .When(exception = CaughtException(() => instantiator.Create<TestClassThatCannotBeInjected>()))

                .Then(exception, 
                    Is(AnException.Of().Type<BindingException>()
                        .Message("Cannot create DivineInject.Test.TestClassThatCannotBeInjected, could not find an injectable constructor because the following types are not injectable: System.String, DivineInject.Test.ITestSecondDependency")))
            ;
        }
    }

    public interface ITestDependency
    { }

    public interface ITestSecondDependency
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

    public class TestClassWithTwoConstructorsWithDependencies
    {
        public TestClassWithTwoConstructorsWithDependencies(ITestDependency dependency, ITestSecondDependency secondDependency)
        {
            Dependency = dependency;
            SecondDependency = secondDependency;
        }

        public TestClassWithTwoConstructorsWithDependencies(ITestDependency dependency)
        {
            Dependency = dependency;
        }

        public ITestDependency Dependency { get; private set; }
        public ITestSecondDependency SecondDependency { get; private set; }
    }

    public class TestClassWithAnUninjectableConstructor
    {
        public TestClassWithAnUninjectableConstructor(string name)
        {
        }

        public TestClassWithAnUninjectableConstructor(ITestDependency dependency)
        {
            Dependency = dependency;
        }

        public ITestDependency Dependency { get; private set; }
    }

    public class TestClassThatCannotBeInjected
    {
        public TestClassThatCannotBeInjected(string name)
        {
        }

        public TestClassThatCannotBeInjected(ITestSecondDependency dependency)
        {
        }
    }
}
