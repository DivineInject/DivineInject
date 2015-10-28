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
            IOrderService instance;

            Scenario()
                .Given(injector = new DivineInjector())
                .Given(() => injector.Bind<IOrderService>().To<OrderService>())

                .When(instance = injector.Get<IOrderService>())

                .Then(instance, Is(AnInstance.OfType<OrderService>()))
            ;
        }

        [Test]
        public void MultipleRequestsForSameInterfaceYieldSameObject()
        {
            DivineInjector injector;
            IOrderService instance1, instance2;

            Scenario()
                .Given(injector = new DivineInjector())
                .Given(() => injector.Bind<IOrderService>().To<OrderService>())

                .When(instance1 = injector.Get<IOrderService>())
                .When(instance2 = injector.Get<IOrderService>())

                .Then(instance1, Is(AnInstance.SameAs(instance2)))
            ;
        }

        [Test]
        public void IsBoundReturnsWhetherABindingExistsForAType()
        {
            DivineInjector injector;

            Scenario()
                .Given(injector = new DivineInjector())
                .Given(() => injector.Bind<IOrderService>().To<OrderService>())

                .Then(injector.IsBound(typeof(IOrderService)), IsTrue())
                .Then(injector.IsBound(typeof(string)), IsFalse())
            ;
        }
    }

    public interface IOrderService
    { }

    public class OrderService : IOrderService
    { }
}
