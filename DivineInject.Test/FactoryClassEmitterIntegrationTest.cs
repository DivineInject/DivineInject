using DivineInject.Test.DummyModel;
using NUnit.Framework;
using TestFirst.Net.Extensions.Moq;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    [TestFixture]
    public class FactoryClassEmitterIntegrationTest : AbstractNUnitMoqScenarioTest
    {
        [Test]
        public void CreatesFactoryForDomainObjectWithInjectedDependency()
        {
            FactoryClassEmitter emitter;
            ICreateDomainObjectWithOneDependency factory;
            IDivineInjector injector;
            IDatabase database;
            DomainObjectWithOneDependency obj;

            Scenario()
                .Given(database = AMock<IDatabase>().Instance)
                .Given(injector = AMock<IDivineInjector>()
                    .WhereMethod(i => i.IsBound(typeof(IDatabase))).Returns(true)
                    .WhereMethod(i => i.Get(typeof(IDatabase))).Returns(database)
                    .Instance)
                .Given(emitter = new FactoryClassEmitter(
                    injector, 
                    typeof(ICreateDomainObjectWithOneDependency), 
                    typeof(DomainObjectWithOneDependency)))

                .When(factory = (ICreateDomainObjectWithOneDependency) emitter.CreateNewObject())
                .When(obj = factory.Create())

                .Then(obj, Is(AnInstance.NotNull()))
                .Then(obj.Database, Is(AnInstance.SameAs(database)))
            ;
        }
    }

    public interface ICreateDomainObjectWithOneDependency
    {
        DomainObjectWithOneDependency Create();
    }
}
