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
            IDivineInjector injector;
            IDatabaseProvider instance;

            Scenario()
                .Given(injector = DivineInjector.Current
                    .Bind<IDatabaseProvider>().To<DatabaseProvider>())

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
                    .BindFactory<User.IFactory>().For<User>())

                .When(userFactory = injector.Get<User.IFactory>())
                .When(user = userFactory.Create("Helen"))

                .Then(user.Name, Is(AString.EqualTo("Helen")))
                .Then(((User)user).DatabaseProvider, Is(AnInstance.NotNull()));
        }
    }
}
