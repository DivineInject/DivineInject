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

                .When(instance = generator.Generate<IFactory, DomainObject>(new List<InjectedDependencyProperty>(), new List<ConstructorArg>()))

                .Then(instance, Is(AnInstance.NotNull()))
            ;
        }

        [Test]
        public void GeneratesClassWithProperties()
        {
            ClassGenerator generator;
            InjectedDependencyProperty property1, property2;
            dynamic instance;

            Scenario()
                .Given(property1 = new InjectedDependencyProperty(typeof(string), "Name", "Bob"))
                .Given(property2 = new InjectedDependencyProperty(typeof(int), "Age", 42))

                .Given(generator = new ClassGenerator())

                .When(instance = generator.Generate<IFactory, DomainObject>(new[] { property1, property2 }, new List<ConstructorArg>()))

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
            InjectedDependencyProperty property1, property2;
            dynamic instance;

            Scenario()
                .Given(property1 = new InjectedDependencyProperty(typeof(string), "Name", "Bob"))
                .Given(property2 = new InjectedDependencyProperty(typeof(int), "Age", 42))

                .Given(generator = new ClassGenerator())

                .When(instance = generator.Generate<IFactory, DomainObject>(new[] { property1, property2 }, new List<ConstructorArg>()))

                .Then(instance, Is(AnInstance.NotNull<IFactory>()))
                .Then(instance.GetType().GetProperties(), Is(AList.InAnyOrder().WithAtLeast(
                    APropertyInfo.With().Name("Name").PropertyType(typeof(string)),
                    APropertyInfo.With().Name("Age").PropertyType(typeof(int))
                )))
                .Then(instance.Name, Is(AString.EqualTo("Bob")))
                .Then(instance.Age, Is(AnInt.EqualTo(42)))
            ;
        }

        [Test]
        public void FactoryMethodOnInterfaceCreatesObjectWithoutConstructorArgs()
        {
            ClassGenerator generator;
            InjectedDependencyProperty property1, property2;
            IFactory factory;
            IDomainObject obj;

            Scenario()
                .Given(property1 = new InjectedDependencyProperty(typeof(string), "Name", "Bob"))
                .Given(property2 = new InjectedDependencyProperty(typeof(int), "Age", 42))

                .Given(generator = new ClassGenerator())

                .When(factory = generator.Generate<IFactory, DomainObject>(new[] { property1, property2 }, new List<ConstructorArg>()))
                .When(obj = factory.Create())

                .Then(obj, Is(AnInstance.NotNull()))
                .Then(obj.DummyMethod(), Is(AString.EqualTo("Hello")))
            ;
        }

        [Test]
        public void FactoryMethodOnInterfaceCreatesObjectWithConstructorArgs()
        {
            ClassGenerator generator;
            InjectedDependencyProperty property1, property2;
            IFactory factory;
            IDomainObject obj;
            ConstructorArg constructorArg1, constructorArg2;

            Scenario()
                .Given(property1 = new InjectedDependencyProperty(typeof(string), "Name", "Bob"))
                .Given(property2 = new InjectedDependencyProperty(typeof(int), "Age", 42))
                .Given(constructorArg1 = new ConstructorArg(typeof(string), 0, null))
                .Given(constructorArg2 = new ConstructorArg(typeof(int), 1, null))

                .Given(generator = new ClassGenerator())

                .When(factory = generator.Generate<IFactory, DomainObject>(
                    new[] { property1, property2 },
                    new[] { constructorArg1, constructorArg2 }))
                .When(obj = factory.Create())

                .Then(obj, Is(AnInstance.NotNull()))
                .Then(obj.DummyMethod(), Is(AString.EqualTo("Hello")))
                .Then(obj.Name, Is(AString.EqualTo("Bob")))
                .Then(obj.Age, Is(AnInt.EqualTo(42)))
            ;
        }

        [Test]
        public void FactoryMethodOnInterfaceCreatesObjectWithConstructorArgsFromFactoryWithArgs()
        {
            ClassGenerator generator;
            InjectedDependencyProperty property1, property2;
            IFactoryWithArg factory;
            IDomainObject obj;
            ConstructorArg constructorArg1, constructorArg2, constructorArg3;

            Scenario()
                .Given(property1 = new InjectedDependencyProperty(typeof(string), "Name", "Bob"))
                .Given(property2 = new InjectedDependencyProperty(typeof(int), "Age", 42))
                .Given(constructorArg1 = new ConstructorArg(typeof(string), 0, null))
                .Given(constructorArg2 = new ConstructorArg(typeof(int), 1, null))
                .Given(constructorArg3 = new ConstructorArg(typeof(string), null, 0))

                .Given(generator = new ClassGenerator())

                .When(factory = generator.Generate<IFactoryWithArg, DomainObject>(
                    new[] { property1, property2 },
                    new[] { constructorArg1, constructorArg2, constructorArg3 }))
                .When(obj = factory.Create("developer"))

                .Then(obj, Is(AnInstance.NotNull()))
                .Then(obj.DummyMethod(), Is(AString.EqualTo("Hello")))
                .Then(obj.Name, Is(AString.EqualTo("Bob")))
                .Then(obj.Age, Is(AnInt.EqualTo(42)))
                .Then(obj.Role, Is(AString.EqualTo("developer")))
            ;
        }
    }

    public interface IFactory
    {
        IDomainObject Create();
    }

    public interface IFactoryWithArg
    {
        IDomainObject Create(string role);
    }

    public interface IDomainObject
    {
        string Name { get; }
        int Age { get; }
        string Role { get; }
        string DummyMethod();
    }

    public class DomainObject : IDomainObject
    {
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Role { get; private set; }

        public DomainObject()
        {
        }

        public DomainObject(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public DomainObject(string name, int age, string role)
        {
            Name = name;
            Age = age;
            Role = role;
        }

        public string DummyMethod()
        {
            return "Hello";
        }
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
