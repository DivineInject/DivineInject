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
    }

    public interface IOrderService
    { }

    public class OrderService : IOrderService
    { }
}
