using DivineInject.Test.DummyModel;
using NUnit.Framework;
using TestFirst.Net.Extensions.Moq;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    [TestFixture]
    public class FactoryClassEmitterIntegrationTest : AbstractNUnitMoqScenarioTest
    {
        [Test]
        public void CreatesFactoryForDomainObjectWithDefaultConstructor()
        {
            FactoryClassEmitter emitter;
            ICreateDomainObjectWithDefaultConstructor factory;
            IDivineInjector injector;
            IDomainObject obj;

            Scenario()
                .Given(injector = AMock<IDivineInjector>().Instance)
                .Given(emitter = new FactoryClassEmitter(
                    injector,
                    typeof(ICreateDomainObjectWithDefaultConstructor),
                    typeof(DomainObjectWithDefaultConstructor)))

                .When(factory = (ICreateDomainObjectWithDefaultConstructor)emitter.CreateNewObject())
                .When(obj = factory.Create())

                .Then(obj, Is(AnInstance.NotNull()))
            ;
        }

        [Test]
        public void CreatesFactoryForDomainObjectWithInjectedDependency()
        {
            FactoryClassEmitter emitter;
            ICreateDomainObjectWithOneDependency factory;
            IDivineInjector injector;
            IDatabase database;
            IDomainObject obj;

            Scenario()
                .Given(database = AMock<IDatabase>().Instance)
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(IDatabase))).Returns(true)
                    .WhereMethod(i => i.Get(typeof(IDatabase))).Returns(database)
                    .Instance)
                .Given(emitter = new FactoryClassEmitter(
                    injector, 
                    typeof(ICreateDomainObjectWithOneDependency), 
                    typeof(DomainObjectWithOneDependency)))

                .When(factory = (ICreateDomainObjectWithOneDependency) emitter.CreateNewObject())
                .When(obj = factory.Create())

                .Then(obj, Is(AnInstance.NotNull()))
                .Then(((DomainObjectWithOneDependency)obj).Database, Is(AnInstance.SameAs(database)))
            ;
        }

        [Test]
        public void CreatesFactoryForDomainObjectWithPassedArgument()
        {
            FactoryClassEmitter emitter;
            ICreateDomainObjectWithSingleArgConstructor factory;
            IDivineInjector injector;
            IDomainObjectWithName obj;

            Scenario()
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(string))).Returns(false)
                    .Instance)
                .Given(emitter = new FactoryClassEmitter(
                    injector,
                    typeof(ICreateDomainObjectWithSingleArgConstructor),
                    typeof(DomainObjectWithSingleArgConstructor)))

                .When(factory = (ICreateDomainObjectWithSingleArgConstructor)emitter.CreateNewObject())
                .When(obj = factory.Create("bob"))

                .Then(obj, Is(AnInstance.NotNull()))
                .Then(obj.Name, Is(AString.EqualTo("bob")))
            ;
        }

        [Test]
        public void CreatesFactoryForDomainObjectWithInjectedDependencyAndArgument()
        {
            FactoryClassEmitter emitter;
            ICreateDomainObjectWithDependencyAndArg factory;
            IDivineInjector injector;
            IDatabase database;
            IDomainObjectWithName obj;

            Scenario()
                .Given(database = AMock<IDatabase>().Instance)
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(IDatabase))).Returns(true)
                    .WhereMethod(i => i.IsBound(typeof(string))).Returns(false)
                    .WhereMethod(i => i.Get(typeof(IDatabase))).Returns(database)
                    .Instance)
                .Given(emitter = new FactoryClassEmitter(
                    injector,
                    typeof(ICreateDomainObjectWithDependencyAndArg),
                    typeof(DomainObjectWithDependencyAndArg)))

                .When(factory = (ICreateDomainObjectWithDependencyAndArg) emitter.CreateNewObject())
                .When(obj = factory.Create("Fred"))

                .Then(obj, Is(AnInstance.NotNull()))
                .Then(((DomainObjectWithDependencyAndArg)obj).Database, Is(AnInstance.SameAs(database)))
                .Then(obj.Name, Is(AString.EqualTo("Fred")))
            ;
        }

        [Test]
        public void CreatesFactoryForDomainObjectTwoConstructors()
        {
            FactoryClassEmitter emitter;
            ICreateDomainObjectWithTwoConstructors factory;
            IDivineInjector injector;
            IDomainObjectWithName objWithDefaultName, objWithSpecificName;

            Scenario()
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(string))).Returns(false)
                    .Instance)
                .Given(emitter = new FactoryClassEmitter(
                    injector,
                    typeof(ICreateDomainObjectWithTwoConstructors),
                    typeof(DomainObjectWithTwoConstructors)))

                .When(factory = (ICreateDomainObjectWithTwoConstructors)emitter.CreateNewObject())
                .When(objWithDefaultName = factory.CreateWithDefaultName())
                .When(objWithSpecificName = factory.CreateWithName("Bob"))

                .Then(objWithSpecificName.Name, Is(AString.EqualTo("Bob")))
                .Then(objWithDefaultName.Name, Is(AString.EqualTo("Fred")))
            ;
        }

        [Test]
        [Ignore("wip")]
        public void CreatesFactoryForDomainObjectWithConstructorWithTwoArgsOfSameType()
        {
            FactoryClassEmitter emitter;
            ICreateDomainObjectWithConstructorWithTwoArgsOfSameType factory;
            IDivineInjector injector;
            DomainObjectWithConstructorWithTwoArgsOfSameType obj;

            Scenario()
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(string))).Returns(false)
                    .Instance)
                .Given(emitter = new FactoryClassEmitter(
                    injector,
                    typeof(ICreateDomainObjectWithConstructorWithTwoArgsOfSameType),
                    typeof(DomainObjectWithConstructorWithTwoArgsOfSameType)))

                .When(factory = (ICreateDomainObjectWithConstructorWithTwoArgsOfSameType)emitter.CreateNewObject())
                .When(obj = factory.Create("developer", "sarah"))

                .Then(obj, Is(AnInstance.NotNull()))
                .Then(obj.Name, Is(AString.EqualTo("sarah")))
                .Then(obj.Role, Is(AString.EqualTo("developer")))
            ;
        }
    }

    public interface ICreateDomainObjectWithDefaultConstructor
    {
        IDomainObject Create();
    }

    public interface ICreateDomainObjectWithOneDependency
    {
        IDomainObject Create();
    }

    public interface ICreateDomainObjectWithSingleArgConstructor
    {
        IDomainObjectWithName Create(string name);
    }

    public interface ICreateDomainObjectWithDependencyAndArg
    {
        IDomainObjectWithName Create(string name);
    }

    public interface ICreateDomainObjectWithTwoConstructors
    {
        IDomainObjectWithName CreateWithDefaultName();
        IDomainObjectWithName CreateWithName(string name);
    }

    public interface ICreateDomainObjectWithConstructorWithTwoArgsOfSameType
    {
        DomainObjectWithConstructorWithTwoArgsOfSameType Create(string role, string name);
    }
}
