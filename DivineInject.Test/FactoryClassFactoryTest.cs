using System.Collections.Generic;
using DivineInject.Test.DummyModel;
using NUnit.Framework;
using TestFirst.Net.Extensions.Moq;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    [TestFixture]
    public class FactoryClassFactoryTest : AbstractNUnitMoqScenarioTest
    {
        [Test]
        public void CreatesFactoryClass()
        {
            IFactoryMethodFactory methodFactory;
            FactoryClassFactory classFactory;
            IDivineInjector injector;
            FactoryClass createdClass;
            IFactoryMethod method1;
            IFactoryMethod method2;

            Scenario()
                .Given(method1 = AMock<IFactoryMethod>()
                    .WhereGet(m => m.ConstructorArgs).Returns(new List<IConstructorArgDefinition>())
                    .Instance)
                .Given(method2 = AMock<IFactoryMethod>()
                    .WhereGet(m => m.ConstructorArgs).Returns(new List<IConstructorArgDefinition>())
                    .Instance)
                .Given(injector = AMock<IDivineInjector>().Instance)
                .Given(methodFactory = AMock<IFactoryMethodFactory>()
                    .WhereMethod(f => f.Create(
                        ArgIs(AMethodInfo.With().Name("MethodWithDependencyOnly")), 
                        injector,
                        typeof(DomainObjectWithDependencyAndArg)))
                        .Returns(method1)
                    .WhereMethod(f => f.Create(
                        ArgIs(AMethodInfo.With().Name("MethodWithDependencyAndArg")),
                        injector,
                        typeof(DomainObjectWithDependencyAndArg)))
                        .Returns(method2)
                    .Instance)
                .Given(classFactory = new FactoryClassFactory(methodFactory))

                .When(createdClass = classFactory.Create(
                    typeof(IFactoryInterfaceWithTwoMethods),
                    injector,
                    typeof(DomainObjectWithDependencyAndArg)))

                .Then(createdClass.Methods, Is(AList.InOrder().WithOnly(
                    AnInstance.SameAs(method1),
                    AnInstance.SameAs(method2)
                )))
            ;
        }
    }

    internal interface IFactoryInterfaceWithTwoMethods
    {
        DomainObjectWithDependencyAndArg MethodWithDependencyOnly();
        DomainObjectWithDependencyAndArg MethodWithDependencyAndArg(string name);
    }
}
