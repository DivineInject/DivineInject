﻿using DivineInject.Test.DummyModel;
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
            DomainObjectWithDefaultConstructor obj;

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
            DomainObjectWithOneDependency obj;

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
                .Then(obj.Database, Is(AnInstance.SameAs(database)))
            ;
        }

        [Test]
        public void CreatesFactoryForDomainObjectWithPassedArgument()
        {
            FactoryClassEmitter emitter;
            ICreateDomainObjectWithSingleArgConstructor factory;
            IDivineInjector injector;
            DomainObjectWithSingleArgConstructor obj;

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
            DomainObjectWithDependencyAndArg obj;

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
                .Then(obj.Database, Is(AnInstance.SameAs(database)))
                .Then(obj.Name, Is(AString.EqualTo("Fred")))
            ;
        }

        [Test]
        public void CreatesFactoryForDomainObjectTwoConstructors()
        {
            FactoryClassEmitter emitter;
            ICreateDomainObjectWithTwoConstructors factory;
            IDivineInjector injector;
            DomainObjectWithTwoConstructors objWithDefaultName, objWithSpecificName;

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
    }

    public interface ICreateDomainObjectWithDefaultConstructor
    {
        DomainObjectWithDefaultConstructor Create();
    }

    public interface ICreateDomainObjectWithOneDependency
    {
        DomainObjectWithOneDependency Create();
    }

    public interface ICreateDomainObjectWithSingleArgConstructor
    {
        DomainObjectWithSingleArgConstructor Create(string name);
    }

    public interface ICreateDomainObjectWithDependencyAndArg
    {
        DomainObjectWithDependencyAndArg Create(string name);
    }

    public interface ICreateDomainObjectWithTwoConstructors
    {
        DomainObjectWithTwoConstructors CreateWithDefaultName();
        DomainObjectWithTwoConstructors CreateWithName(string name);
    }
}
