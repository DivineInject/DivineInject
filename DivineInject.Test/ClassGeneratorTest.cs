using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestFirst.Net.Extensions.NUnit;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    [TestFixture]
    public class ClassGeneratorTest : AbstractNUnitScenarioTest
    {
        [Test]
        public void GeneratesBasicClass()
        {
            ClassGenerator generator;
            object instance;

            Scenario()
                .Given(generator = new ClassGenerator())

                .When(instance = generator.Generate())

                .Then(instance, Is(AnInstance.NotNull()))
            ;
        }
    }
}
