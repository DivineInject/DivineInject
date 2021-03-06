﻿using System;
using NUnit.Framework;
using TestFirst.Net.Extensions.Moq;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    [TestFixture]
    public class DivineInjectorTest : AbstractNUnitMoqScenarioTest
    {
        [TearDown]
        public void ResetInjector()
        {
            DivineInjector.Current.Reset();
        }

        [Test]
        public void BindsInterfaceToImplementationAndInstantiates()
        {
            IDivineInjector injector;
            IDatabaseProvider instance;

            Scenario()
                .Given(injector = DivineInjector.Current
                    .Bind<IDatabaseProvider>().To<DatabaseProvider>())

                .When(instance = injector.Get<IDatabaseProvider>())

                .Then(instance, Is(AnInstance.OfType<DatabaseProvider>()));
        }

        [Test]
        public void BindsUsingTypeArgumentToImplementationAndInstantiates()
        {
            IDivineInjector injector;
            IDatabaseProvider instance;

            Scenario()
                .Given(injector = DivineInjector.Current
                    .Bind(typeof(IDatabaseProvider)).To(typeof(DatabaseProvider)))

                .When(instance = injector.Get<IDatabaseProvider>())

                .Then(instance, Is(AnInstance.OfType<DatabaseProvider>()));
        }

        [Test]
        public void MultipleRequestsForSameInterfaceYieldSameObject()
        {
            IDivineInjector injector;
            IDatabaseProvider instance1, instance2;

            Scenario()
                .Given(injector = DivineInjector.Current
                    .Bind<IDatabaseProvider>().To<DatabaseProvider>())

                .When(instance1 = injector.Get<IDatabaseProvider>())
                .When(instance2 = injector.Get<IDatabaseProvider>())

                .Then(instance1, Is(AnInstance.SameAs(instance2)));
        }

        [Test]
        public void IsBoundReturnsWhetherABindingExistsForAType()
        {
            IDivineInjector injector;

            Scenario()
                .Given(injector = DivineInjector.Current
                    .Bind<IDatabaseProvider>().To<DatabaseProvider>())

                .Then(injector.IsBound(typeof(IDatabaseProvider)), IsTrue())
                .Then(injector.IsBound(typeof(string)), IsFalse());
        }

        [Test]
        public void BindsAndInstantiatesDependenciesWithDependencies()
        {
            IDivineInjector injector;
            IOrderService service;

            Scenario()
                .Given(injector = DivineInjector.Current
                    .Bind<IDatabaseProvider>().To<DatabaseProvider>()
                    .Bind<IOrderService>().To<OrderService>())

                .When(service = injector.Get<IOrderService>())

                .Then(service, Is(AnInstance.NotNull()))
                .Then(((OrderService)service).DatabaseProvider, Is(AnInstance.NotNull()));
        }

        [Test]
        public void BindsAndInstantiatesDependencyViaFactoryInterface()
        {
            IDivineInjector injector;
            User.IFactory userFactory;
            IUser user;

            Scenario()
                .Given(injector = DivineInjector.Current
                    .Bind<IDatabaseProvider>().To<DatabaseProvider>()
                    .Bind<User.IFactory>().AsGeneratedFactoryFor<User>())

                .When(userFactory = injector.Get<User.IFactory>())
                .When(user = userFactory.Create("Helen"))

                .Then(user.Name, Is(AString.EqualTo("Helen")))
                .Then(((User)user).DatabaseProvider, Is(AnInstance.NotNull()));
        }

        [Test]
        public void BindsInterfaceToMockInstance()
        {
            IDivineInjector injector;
            IDatabaseProvider instance;
            IDatabaseProvider mockedDatabaseProvider;

            Scenario()
                .Given(mockedDatabaseProvider = AMock<IDatabaseProvider>().Instance)
                .Given(injector = DivineInjector.Current
                    .Bind<IDatabaseProvider>().ToInstance(mockedDatabaseProvider))

                .When(instance = injector.Get<IDatabaseProvider>())

                .Then(instance, Is(AnInstance.SameAs(mockedDatabaseProvider)));
        }

        [Test]
        public void AutoBindsAClassThatCanBeConstructedFromDependencies()
        {
            IDivineInjector injector;
            IOrderService service;

            Scenario()
                .Given(injector = DivineInjector.Current
                    .Bind<IDatabaseProvider>().To<DatabaseProvider>())

                .When(service = injector.Get<OrderService>())

                .Then(service, Is(AnInstance.NotNull()))
                .Then(((OrderService)service).DatabaseProvider, Is(AnInstance.NotNull()));
        }

        [Test]
        public void WhenAnUnboundClassCannotBeInstantiatedThrowsUsefulException()
        {
            IDivineInjector injector;
            Exception exception;

            Scenario()
                .Given(injector = DivineInjector.Current)

                .When(exception = CaughtException(() => injector.Get<OrderService>()))

                .Then(exception, Is(AnException.With()
                    .Message(AString.Containing("Cannot create DivineInject.Test.OrderService, could not find an injectable constructor because the following types are not injectable: DivineInject.Test.IDatabaseProvider"))));
        }
    }
}
