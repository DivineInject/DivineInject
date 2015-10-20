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
                .Then(factoryMethod.Properties, Is(AList.NoItems<InjectableConstructorArg>()))
                .Then(factoryMethod.Name, Is(AString.EqualTo("MethodWithNoArgs")))
                .Then(factoryMethod.ReturnType, Is(AType.EqualTo(typeof(DomainObjectWithDefaultConstructor))))
                .Then(factoryMethod.ConstructorArgs, Is(AList.NoItems<IConstructorArg>()))
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
                .Then(factoryMethod.Properties, Is(AList.NoItems<InjectableConstructorArg>()))
                .Then(factoryMethod.Name, Is(AString.EqualTo("MethodWithSinglePassedArg")))
                .Then(factoryMethod.ReturnType, Is(AType.EqualTo(typeof(DomainObjectWithSingleArgConstructor))))
                .Then(factoryMethod.ConstructorArgs, Is(AList.InOrder().WithOnly(
                    APassedConstructorArg.With().Type(typeof(string))
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
                .Then(factoryMethod.Properties, Is(AList.InOrder().WithOnly(
                    AnInjectableConstructorArg.With()
                        .Name("Database")
                        .PropertyType(typeof(IDatabase))
                )))
                .Then(factoryMethod.Name, Is(AString.EqualTo("MethodWithSingleDependency")))
                .Then(factoryMethod.ReturnType, Is(AType.EqualTo(typeof(DomainObjectWithOneDependency))))
                .Then(factoryMethod.ConstructorArgs, Is(AList.InOrder().WithOnly(  
                    AnInjectableConstructorArg.With().Name("Database").PropertyType(typeof(IDatabase))
                )))
            ;
        }

        [Test]
        public void CreatesFactoryMethodForMethodWithOneArgAndAnInjectableDependency()
        {
            FactoryMethodFactory factoryMethodFactory;
            MethodInfo methodInfo;
            IDivineInjector injector;
            IFactoryMethod factoryMethod;
            Type domainObjectType;
            ConstructorInfo expectedConstructor;

            Scenario()
                .Given(factoryMethodFactory = new FactoryMethodFactory())
                .Given(methodInfo = typeof(IDummyFactory).GetMethod("MethodWithDependencyAndArg"))
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(string))).Returns(false)
                    .WhereMethod(i => i.IsBound(typeof(IDatabase))).Returns(true)
                    .Instance)
                .Given(domainObjectType = typeof(DomainObjectWithDependencyAndArg))
                .Given(expectedConstructor = domainObjectType.GetConstructor(new[] { typeof(IDatabase), typeof(string) }))

                .When(factoryMethod = factoryMethodFactory.Create(methodInfo, injector, domainObjectType))

                .Then(factoryMethod.Constructor, Is(AnInstance.SameAs(expectedConstructor)))
                .Then(factoryMethod.Properties, Is(AList.InOrder().WithOnly(
                    AnInjectableConstructorArg.With()
                        .Name("Database")
                        .PropertyType(typeof(IDatabase))
                )))
                .Then(factoryMethod.Name, Is(AString.EqualTo("MethodWithDependencyAndArg")))
                .Then(factoryMethod.ReturnType, Is(AType.EqualTo(typeof(DomainObjectWithDependencyAndArg))))
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
    }

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
