using NUnit.Framework;
using TestFirst.Net.Extensions.NUnit;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    [TestFixture]
    public class DivineInjectorTest : AbstractNUnitScenarioTest
    {
        [Test]
        public void BindsInterfaceToImplementationAndInstantiates()
        {
            DivineInjector injector;
            IDatabaseProvider instance;

            Scenario()
                .Given(injector = new DivineInjector())
                .Given(() => injector.Bind<IDatabaseProvider>().To<DatabaseProvider>())

                .When(instance = injector.Get<IDatabaseProvider>())

                .Then(instance, Is(AnInstance.OfType<DatabaseProvider>()))
            ;
        }

        [Test]
        public void MultipleRequestsForSameInterfaceYieldSameObject()
        {
            DivineInjector injector;
            IDatabaseProvider instance1, instance2;

            Scenario()
                .Given(injector = new DivineInjector())
                .Given(() => injector.Bind<IDatabaseProvider>().To<DatabaseProvider>())

                .When(instance1 = injector.Get<IDatabaseProvider>())
                .When(instance2 = injector.Get<IDatabaseProvider>())

                .Then(instance1, Is(AnInstance.SameAs(instance2)))
            ;
        }

        [Test]
        public void IsBoundReturnsWhetherABindingExistsForAType()
        {
            DivineInjector injector;

            Scenario()
                .Given(injector = new DivineInjector())
                .Given(() => injector.Bind<IDatabaseProvider>().To<DatabaseProvider>())

                .Then(injector.IsBound(typeof(IDatabaseProvider)), IsTrue())
                .Then(injector.IsBound(typeof(string)), IsFalse())
            ;
        }

        [Test]
        public void BindsAndInstantiatesDependenciesWithDependencies()
        {
            DivineInjector injector;
            IOrderService service;

            Scenario()
                .Given(injector = new DivineInjector())
                .Given(() => injector.Bind<IDatabaseProvider>().To<DatabaseProvider>())
                .Given(() => injector.Bind<IOrderService>().To<OrderService>())

                .When(service = injector.Get<IOrderService>())

                .Then(service, Is(AnInstance.NotNull()))
                .Then(((OrderService) service).DatabaseProvider, Is(AnInstance.NotNull()))
            ;
        }

        [Test]
        public void BindsAndInstantiatesDependencyViaFactoryInterface()
        {
            DivineInjector injector;
            User.IFactory userFactory;
            IUser user;

            Scenario()
                .Given(injector = new DivineInjector())
                .Given(() => injector.Bind<IDatabaseProvider>().To<DatabaseProvider>())
                .Given(() => injector.BindFactory<User.IFactory>().For<User>())

                .When(userFactory = injector.Get<User.IFactory>())
                .When(user = userFactory.Create("Helen"))

                .Then(user.Name, Is(AString.EqualTo("Helen")))
                .Then(((User)user).DatabaseProvider, Is(AnInstance.NotNull()))
            ;
        }
    }

    public interface IDatabaseProvider
    { }

    public class DatabaseProvider : IDatabaseProvider
    { }

    public interface IOrderService
    { }

    public class OrderService : IOrderService
    {
        public IDatabaseProvider DatabaseProvider { get; private set; }

        public OrderService(IDatabaseProvider databaseProvider)
        {
            DatabaseProvider = databaseProvider;
        }
    }

    public interface IUser
    {
        string Name { get; }
    }

    public class User : IUser
    {
        public User(string name, IDatabaseProvider databaseProvider)
        {
            Name = name;
            DatabaseProvider = databaseProvider;
        }

        public string Name { get; private set; }
        public IDatabaseProvider DatabaseProvider { get; private set; }

        public interface IFactory
        {
            IUser Create(string name);
        }
    }
}
