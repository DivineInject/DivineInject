using System;
using System.Reflection;
using DivineInject.Test.DummyModel;
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
            IFactoryMethod factoryMethod;
            Type domainObjectType;

            Scenario()
                .Given(factoryMethodFactory = new FactoryMethodFactory())
                .Given(methodInfo = typeof (IDummyFactory).GetMethod("MethodWithNoArgs"))
                .Given(injector = AMock<IDivineInjector>().Instance)
                .Given(domainObjectType = typeof(DomainObjectWithDefaultConstructor))

                .When(factoryMethod = factoryMethodFactory.Create(methodInfo, injector, domainObjectType))

                .Then(factoryMethod.Constructor, Is(AnInstance.SameAs(domainObjectType.GetConstructor(new Type[0]))))
                .Then(factoryMethod.Name, Is(AString.EqualTo("MethodWithNoArgs")))
                .Then(factoryMethod.ReturnType, Is(AType.EqualTo(typeof(DomainObjectWithDefaultConstructor))))
                .Then(factoryMethod.Parameters, Is(AList.NoItems<Type>()))
                .Then(factoryMethod.ConstructorArgs, Is(AList.NoItems<IConstructorArgDefinition>()))
            ;
        }

        [Test]
        public void CreatesFactoryMethodForMethodWithOneArgAndMatchingConstructorInTarget()
        {
            FactoryMethodFactory factoryMethodFactory;
            MethodInfo methodInfo;
            IDivineInjector injector;
            IFactoryMethod factoryMethod;
            Type domainObjectType;
            ConstructorInfo expectedConstructor;

            Scenario()
                .Given(factoryMethodFactory = new FactoryMethodFactory())
                .Given(methodInfo = typeof(IDummyFactory).GetMethod("MethodWithSinglePassedArg"))
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(string))).Returns(false)
                    .Instance)
                .Given(domainObjectType = typeof(DomainObjectWithSingleArgConstructor))
                .Given(expectedConstructor = domainObjectType.GetConstructor(new []{typeof(string)}))

                .When(factoryMethod = factoryMethodFactory.Create(methodInfo, injector, domainObjectType))

                .Then(factoryMethod.Constructor, Is(AnInstance.SameAs(expectedConstructor)))
                .Then(factoryMethod.Name, Is(AString.EqualTo("MethodWithSinglePassedArg")))
                .Then(factoryMethod.ReturnType, Is(AType.EqualTo(typeof(DomainObjectWithSingleArgConstructor))))
                .Then(factoryMethod.Parameters, Is(AList.InOrder().WithOnlyValues(typeof(string))))
                .Then(factoryMethod.ConstructorArgs, Is(AList.InOrder().WithOnly(
                    APassedConstructorArgDefinition.With().Type(typeof(string))
                )))
            ;
        }

        [Test]
        public void CreatesFactoryMethodForMethodWithAnInjectableDependency()
        {
            FactoryMethodFactory factoryMethodFactory;
            MethodInfo methodInfo;
            IDivineInjector injector;
            IFactoryMethod factoryMethod;
            Type domainObjectType;
            ConstructorInfo expectedConstructor;

            Scenario()
                .Given(factoryMethodFactory = new FactoryMethodFactory())
                .Given(methodInfo = typeof(IDummyFactory).GetMethod("MethodWithSingleDependency"))
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(IDatabase))).Returns(true)
                    .Instance)
                .Given(domainObjectType = typeof(DomainObjectWithOneDependency))
                .Given(expectedConstructor = domainObjectType.GetConstructor(new[] { typeof(IDatabase) }))

                .When(factoryMethod = factoryMethodFactory.Create(methodInfo, injector, domainObjectType))

                .Then(factoryMethod.Constructor, Is(AnInstance.SameAs(expectedConstructor)))
                .Then(factoryMethod.Name, Is(AString.EqualTo("MethodWithSingleDependency")))
                .Then(factoryMethod.ReturnType, Is(AType.EqualTo(typeof(DomainObjectWithOneDependency))))
                .Then(factoryMethod.Parameters, Is(AList.NoItems<Type>()))
                .Then(factoryMethod.ConstructorArgs, Is(AList.InOrder().WithOnly(  
                    AnInjectableConstructorArgDefinition.With().Name("Database").PropertyType(typeof(IDatabase))
                )))
            ;
        }

        [Test]
        public void CreatesFactoryMethodForMethodWithTwoArgsAndAnInjectableDependency()
        {
            FactoryMethodFactory factoryMethodFactory;
            MethodInfo methodInfo;
            IDivineInjector injector;
            IFactoryMethod factoryMethod;
            Type domainObjectType;
            ConstructorInfo expectedConstructor;

            Scenario()
                .Given(factoryMethodFactory = new FactoryMethodFactory())
                .Given(methodInfo = typeof(IDummyFactory).GetMethod("MethodWithDependencyAndTwoArgs"))
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(string))).Returns(false)
                    .WhereMethod(i => i.IsBound(typeof(int))).Returns(false)
                    .WhereMethod(i => i.IsBound(typeof(IDatabase))).Returns(true)
                    .Instance)
                .Given(domainObjectType = typeof(DomainObjectWithDependencyAndTwoArgs))
                .Given(expectedConstructor = domainObjectType.GetConstructor(new[] { typeof(IDatabase), typeof(string), typeof(int) }))

                .When(factoryMethod = factoryMethodFactory.Create(methodInfo, injector, domainObjectType))

                .Then(factoryMethod.Constructor, Is(AnInstance.SameAs(expectedConstructor)))
                .Then(factoryMethod.Name, Is(AString.EqualTo("MethodWithDependencyAndTwoArgs")))
                .Then(factoryMethod.ReturnType, Is(AType.EqualTo(typeof(DomainObjectWithDependencyAndTwoArgs))))
                .Then(factoryMethod.Parameters, Is(AList.InOrder().WithOnlyValues(typeof(string), typeof(int))))
                .Then(factoryMethod.ConstructorArgs, Is(AMixedList.Of<IConstructorArgDefinition>().With(
                    AnInjectableConstructorArgDefinition.With().Name("Database").PropertyType(typeof(IDatabase)),
                    APassedConstructorArgDefinition.With().Type(typeof(string)).Index(0),
                    APassedConstructorArgDefinition.With().Type(typeof(int)).Index(1)
                )))
            ;
        }

        [Test]
        public void CreateThrowsExceptionInCaseNoSuitableConstructorFound()
        {
            FactoryMethodFactory factoryMethodFactory;
            MethodInfo methodInfo;
            IDivineInjector injector;
            Type domainObjectType;
            Exception exception;

            Scenario()
                .Given(factoryMethodFactory = new FactoryMethodFactory())
                .Given(methodInfo = typeof(IDummyFactory).GetMethod("MethodWithSingleDependency"))
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(IDatabase))).Returns(false)
                    .Instance)
                .Given(domainObjectType = typeof(DomainObjectWithOneDependency))

                .When(exception = CaughtException(() => factoryMethodFactory.Create(methodInfo, injector, domainObjectType)))

                .Then(exception, Is(AnException.With().Message("Could not find constructor on DomainObjectWithOneDependency for factory method IDummyFactory.MethodWithSingleDependency")))
            ;
        }
    }

    internal interface IDummyFactory
    {
        DomainObjectWithDefaultConstructor MethodWithNoArgs();
        DomainObjectWithSingleArgConstructor MethodWithSinglePassedArg(string name);
        DomainObjectWithOneDependency MethodWithSingleDependency();
        DomainObjectWithDependencyAndArg MethodWithDependencyAndArg(string name);
        DomainObjectWithDependencyAndTwoArgs MethodWithDependencyAndTwoArgs(string name, int timeout);
    }
}
