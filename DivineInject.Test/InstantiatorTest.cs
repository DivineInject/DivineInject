using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestFirst.Net.Extensions.Moq;
using TestFirst.Net.Matcher;
using DivineInject;

namespace DivineInject.Test
{
    [TestFixture]
    public class InstantiatorTest : AbstractNUnitMoqScenarioTest
    {
        [Test]
        public void InstantiatesUsingNoArgConstructor()
        {
            Instantiator instantiator;
            object instance;

            Scenario()
                .Given(instantiator = new Instantiator())

                .When(instance = instantiator.Create<TestClassWithNoArgConstructor>())

                .Then((TestClassWithNoArgConstructor) instance, Is(AnInstance.NotNull<TestClassWithNoArgConstructor>()))
            ;
        }
    }

    public class TestClassWithNoArgConstructor
    {
    }
}
