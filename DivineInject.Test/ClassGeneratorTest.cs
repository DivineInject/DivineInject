using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestFirst.Net.Extensions.NUnit;
using TestFirst.Net.Matcher;
using System.Reflection;
using TestFirst.Net;

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

                .When(instance = generator.Generate<IFactory, DomainObject>(new List<GeneratedProperty>()))

                .Then(instance, Is(AnInstance.NotNull()))
            ;
        }

        [Test]
        public void GeneratesClassWithProperties()
        {
            ClassGenerator generator;
            GeneratedProperty property1, property2;
            dynamic instance;

            Scenario()
                .Given(property1 = new GeneratedProperty(typeof(string), "Name", "Bob"))
                .Given(property2 = new GeneratedProperty(typeof(int), "Age", 42))

                .Given(generator = new ClassGenerator())

                .When(instance = generator.Generate<IFactory, DomainObject>(new[] { property1, property2 }))

                .Then(instance, Is(AnInstance.NotNull()))
                .Then(instance.GetType().GetProperties(), Is(AList.InAnyOrder().WithAtLeast(
                    APropertyInfo.With().Name("Name").PropertyType(typeof(string)),
                    APropertyInfo.With().Name("Age").PropertyType(typeof(int))
                )))
                .Then(instance.Name, Is(AString.EqualTo("Bob")))
                .Then(instance.Age, Is(AnInt.EqualTo(42)))
            ;
        }

        [Test]
        public void GeneratesClassImplementingInterface()
        {
            ClassGenerator generator;
            GeneratedProperty property1, property2;
            dynamic instance;

            Scenario()
                .Given(property1 = new GeneratedProperty(typeof(string), "Name", "Bob"))
                .Given(property2 = new GeneratedProperty(typeof(int), "Age", 42))

                .Given(generator = new ClassGenerator())

                .When(instance = generator.Generate<IFactory, DomainObject>(new[] { property1, property2 }))

                .Then(instance, Is(AnInstance.NotNull<IFactory>()))
                .Then(instance.GetType().GetProperties(), Is(AList.InAnyOrder().WithAtLeast(
                    APropertyInfo.With().Name("Name").PropertyType(typeof(string)),
                    APropertyInfo.With().Name("Age").PropertyType(typeof(int))
                )))
                .Then(instance.Name, Is(AString.EqualTo("Bob")))
                .Then(instance.Age, Is(AnInt.EqualTo(42)))
            ;
        }
    }

    public interface IFactory
    {
        IDomainObject Create();
    }

    public interface IDomainObject
    {

    }

    public class DomainObject
    {

    }

    internal class APropertyInfo : PropertyMatcher<PropertyInfo>
    {
        private static readonly PropertyInfo PropertyNames = null;

        private APropertyInfo() { }

        public static APropertyInfo With()
        {
            return new APropertyInfo();
        }

        public APropertyInfo Name(string name)
        {
            return Name(AString.EqualTo(name));
        }

        public APropertyInfo Name(IMatcher<string> name)
        {
            WithProperty(() => PropertyNames.Name, name);
            return this;
        }

        public APropertyInfo PropertyType(Type type)
        {
            return PropertyType(AnInstance.EqualTo(type));
        }

        public APropertyInfo PropertyType(IMatcher<Type> type)
        {
            WithProperty(() => PropertyNames.PropertyType, type);
            return this;
        }
    }
}
