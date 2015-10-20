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
                .Then(factoryMethod.Properties, Is(AList.NoItems<InjectedDependencyProperty>()))
            ;
        }

        [Test]
        public void CreatesFactoryMethodForMethodWithOneArgAndMatchingConstructorInTarget()
        {
            FactoryMethodFactory factoryMethodFactory;
            MethodInfo methodInfo;
            IDivineInjector injector;
            FactoryMethod factoryMethod;
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
                .Then(factoryMethod.Properties, Is(AList.NoItems<InjectedDependencyProperty>()))
            ;
        }

        [Test]
        public void CreatesFactoryMethodForMethodWithAnInjectableDependency()
        {
            FactoryMethodFactory factoryMethodFactory;
            MethodInfo methodInfo;
            IDivineInjector injector;
            FactoryMethod factoryMethod;
            Type domainObjectType;
            ConstructorInfo expectedConstructor;
            IDatabase database;

            Scenario()
                .Given(factoryMethodFactory = new FactoryMethodFactory())
                .Given(methodInfo = typeof(IDummyFactory).GetMethod("MethodWithSingleDependency"))
                .Given(database = AMock<IDatabase>().Instance)
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(IDatabase))).Returns(true)
                    .WhereMethod(i => i.Get(typeof(IDatabase))).Returns(database)
                    .Instance)
                .Given(domainObjectType = typeof(DomainObjectWithOneDependency))
                .Given(expectedConstructor = domainObjectType.GetConstructor(new[] { typeof(IDatabase) }))

                .When(factoryMethod = factoryMethodFactory.Create(methodInfo, injector, domainObjectType))

                .Then(factoryMethod.Constructor, Is(AnInstance.SameAs(expectedConstructor)))
                .Then(factoryMethod.Properties, Is(AList.InOrder().WithOnly(
                    AnInjectedDependencyProperty.With()
                        .Name("Database")
                        .PropertyType(typeof(IDatabase))
                        .PropertyValue(database)
                )))
            ;
        }

        [Test]
        public void CreatesFactoryMethodForMethodWithOneArgAndAnInjectableDependency()
        {
            FactoryMethodFactory factoryMethodFactory;
            MethodInfo methodInfo;
            IDivineInjector injector;
            FactoryMethod factoryMethod;
            Type domainObjectType;
            ConstructorInfo expectedConstructor;
            IDatabase database;

            Scenario()
                .Given(factoryMethodFactory = new FactoryMethodFactory())
                .Given(methodInfo = typeof(IDummyFactory).GetMethod("MethodWithDependencyAndArg"))
                .Given(database = AMock<IDatabase>().Instance)
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(string))).Returns(false)
                    .WhereMethod(i => i.IsBound(typeof(IDatabase))).Returns(true)
                    .WhereMethod(i => i.Get(typeof(IDatabase))).Returns(database)
                    .Instance)
                .Given(domainObjectType = typeof(DomainObjectWithDependencyAndArg))
                .Given(expectedConstructor = domainObjectType.GetConstructor(new[] { typeof(IDatabase), typeof(string) }))

                .When(factoryMethod = factoryMethodFactory.Create(methodInfo, injector, domainObjectType))

                .Then(factoryMethod.Constructor, Is(AnInstance.SameAs(expectedConstructor)))
                .Then(factoryMethod.Properties, Is(AList.InOrder().WithOnly(
                    AnInjectedDependencyProperty.With()
                        .Name("Database")
                        .PropertyType(typeof(IDatabase))
                        .PropertyValue(database)
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

    public interface IDatabase
    {
    }

    internal interface IDummyFactory
    {
        string MethodWithNoArgs();
        DomainObjectWithSingleArgConstructor MethodWithSinglePassedArg(string name);
        DomainObjectWithOneDependency MethodWithSingleDependency();
        DomainObjectWithDependencyAndArg MethodWithDependencyAndArg(string name);
    }

    internal class DomainObjectWithDefaultConstructor
    {
    }

    internal class DomainObjectWithSingleArgConstructor
    {
        public DomainObjectWithSingleArgConstructor()
        {
            throw new NotImplementedException();
        }

        public DomainObjectWithSingleArgConstructor(string name)
        {
            
        }
    }

    internal class DomainObjectWithOneDependency
    {
        public DomainObjectWithOneDependency(IDatabase database)
        {
        }
    }

    internal class DomainObjectWithDependencyAndArg
    {
        public DomainObjectWithDependencyAndArg(IDatabase database, string name)
        {
        }
    }
}
