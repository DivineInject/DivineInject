using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestFirst.Net.Extensions.NUnit;
using DivineInject;

namespace DivineInject.Test
{
    [TestFixture]
    public class InstantiatorTest : AbstractNUnitScenarioTest
    {
        [Test]
        [Ignore("wip")]
        public void InstantiatesUsingNoArgConstructor()
        {
            Instantiator instantiator;

            Scenario()
                .Given(instantiator = new Instantiator())
            ;
        }
    }
}
