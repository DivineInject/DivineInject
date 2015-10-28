﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using TestFirst.Net.Matcher;
using System.Reflection;
using TestFirst.Net;
using TestFirst.Net.Extensions.Moq;

namespace DivineInject.Test
{
    [TestFixture]
    public class ClassGeneratorTest : AbstractNUnitMoqScenarioTest
    {
        [Test]
        public void GeneratesBasicClass()
        {
            ClassGenerator generator;
            object instance;
            IDivineInjector injector;

            Scenario()
                .Given(injector = AMock<IDivineInjector>().Instance)
                .Given(generator = new ClassGenerator())

                .When(instance = generator.Generate<IFactory, DomainObject>(
                    new List<InjectableConstructorArgDefinition>(), new List<LegacyConstructorArg>(), injector))

                .Then(instance, Is(AnInstance.NotNull()))
            ;
        }

        [Test]
        public void GeneratesClassWithProperties()
        {
            ClassGenerator generator;
            InjectableConstructorArgDefinition property1, property2;
            dynamic instance;
            IDivineInjector injector;

            Scenario()
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.Get(typeof(string))).Returns("Bob")
                    .WhereMethod(i => i.Get(typeof(int))).Returns(42)
                    .Instance)
                .Given(property1 = new InjectableConstructorArgDefinition(typeof(string), "Name"))
                .Given(property2 = new InjectableConstructorArgDefinition(typeof(int), "Age"))

                .Given(generator = new ClassGenerator())

                .When(instance = generator.Generate<IFactory, DomainObject>(
                    new[] { property1, property2 }, 
                    new List<LegacyConstructorArg>(),
                    injector))

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
            InjectableConstructorArgDefinition property1, property2;
            dynamic instance;
            IDivineInjector injector;

            Scenario()
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.Get(typeof(string))).Returns("Bob")
                    .WhereMethod(i => i.Get(typeof(int))).Returns(42)
                    .Instance)
                .Given(property1 = new InjectableConstructorArgDefinition(typeof(string), "Name"))
                .Given(property2 = new InjectableConstructorArgDefinition(typeof(int), "Age"))

                .Given(generator = new ClassGenerator())

                .When(instance = generator.Generate<IFactory, DomainObject>(
                    new[] { property1, property2 }, 
                    new List<LegacyConstructorArg>(),
                    injector))

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
            InjectableConstructorArgDefinition property1, property2;
            IFactory factory;
            IDomainObject obj;
            IDivineInjector injector;

            Scenario()
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.Get(typeof(string))).Returns("Bob")
                    .WhereMethod(i => i.Get(typeof(int))).Returns(42)
                    .Instance)
                .Given(property1 = new InjectableConstructorArgDefinition(typeof(string), "Name"))
                .Given(property2 = new InjectableConstructorArgDefinition(typeof(int), "Age"))

                .Given(generator = new ClassGenerator())

                .When(factory = generator.Generate<IFactory, DomainObject>(
                    new[] { property1, property2 }, 
                    new List<LegacyConstructorArg>(),
                    injector))
                .When(obj = factory.Create())

                .Then(obj, Is(AnInstance.NotNull()))
                .Then(obj.DummyMethod(), Is(AString.EqualTo("Hello")))
            ;
        }

        [Test]
        public void FactoryMethodOnInterfaceCreatesObjectWithConstructorArgs()
        {
            ClassGenerator generator;
            InjectableConstructorArgDefinition property1, property2;
            IFactory factory;
            IDomainObject obj;
            LegacyConstructorArg constructorArg1, constructorArg2;
            IDivineInjector injector;

            Scenario()
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.Get(typeof(string))).Returns("Bob")
                    .WhereMethod(i => i.Get(typeof(int))).Returns(42)
                    .Instance)
                .Given(property1 = new InjectableConstructorArgDefinition(typeof(string), "Name"))
                .Given(property2 = new InjectableConstructorArgDefinition(typeof(int), "Age"))
                .Given(constructorArg1 = new LegacyConstructorArg(typeof(string), 0, null))
                .Given(constructorArg2 = new LegacyConstructorArg(typeof(int), 1, null))

                .Given(generator = new ClassGenerator())

                .When(factory = generator.Generate<IFactory, DomainObject>(
                    new[] { property1, property2 },
                    new[] { constructorArg1, constructorArg2 },
                    injector))
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
            InjectableConstructorArgDefinition property1, property2;
            IFactoryWithArg factory;
            IDomainObject obj;
            LegacyConstructorArg constructorArg1, constructorArg2, constructorArg3;
            IDivineInjector injector;

            Scenario()
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.Get(typeof(string))).Returns("Bob")
                    .WhereMethod(i => i.Get(typeof(int))).Returns(42)
                    .Instance)
                .Given(property1 = new InjectableConstructorArgDefinition(typeof(string), "Name"))
                .Given(property2 = new InjectableConstructorArgDefinition(typeof(int), "Age"))
                .Given(constructorArg1 = new LegacyConstructorArg(typeof(string), 0, null))
                .Given(constructorArg2 = new LegacyConstructorArg(typeof(int), 1, null))
                .Given(constructorArg3 = new LegacyConstructorArg(typeof(string), null, 0))

                .Given(generator = new ClassGenerator())

                .When(factory = generator.Generate<IFactoryWithArg, DomainObject>(
                    new[] { property1, property2 },
                    new[] { constructorArg1, constructorArg2, constructorArg3 },
                    injector))
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
