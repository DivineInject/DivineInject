using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestFirst.Net.Extensions.NUnit;
using TestFirst.Net.Matcher;
using DivineInject;

namespace DivingInject.Test
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
    }

    public interface IOrderService
    { }

    public class OrderService : IOrderService
    { }
}
