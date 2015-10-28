using System.Collections.Generic;
using System.Reflection.Emit;
using NUnit.Framework;
using TestFirst.Net.Extensions.Moq;
using TestFirst.Net.Matcher;

namespace DivineInject.Test
{
    [TestFixture]
    public class ConstructorArgListTest : AbstractNUnitMoqScenarioTest
    {
        [Test]
        public void EmptyList()
        {
            ConstructorArgList argList;
            Scenario()
                .Given(argList = new ConstructorArgList(null))

                .Then(argList.Arguments, Is(AList.NoItems<IConstructorArg>()));
        }

        [Test]
        public void OnePassedArg()
        {
            ConstructorArgList argList;
            IPassedConstructorArgDefinition argDef1;
            IConstructorArg arg1;

            Scenario()
                .Given(arg1 = AMock<IConstructorArg>().Instance)
                .Given(argDef1 = AMock<IPassedConstructorArgDefinition>()
                    .WhereMethod(d => d.FindExisting(ArgIsAny<IList<IConstructorArg>>())).ReturnsNull()
                    .WhereMethod(d => d.Define(ArgIsAny<TypeBuilder>())).Returns(arg1)
                    .Instance)
                .Given(argList = new ConstructorArgList(null, argDef1))

                .Then(argList.Arguments, Is(AList.InOrder().WithOnly(AnInstance.SameAs(arg1))));
        }

        [Test]
        public void OneInjectableArg()
        {
            ConstructorArgList argList;
            IInjectableConstructorArgDefinition argDef1;
            IConstructorArg arg1;

            Scenario()
                .Given(arg1 = AMock<IConstructorArg>().Instance)
                .Given(argDef1 = AMock<IInjectableConstructorArgDefinition>()
                    .WhereMethod(d => d.FindExisting(ArgIsAny<IList<IConstructorArg>>())).ReturnsNull()
                    .WhereMethod(d => d.Define(ArgIsAny<TypeBuilder>())).Returns(arg1)
                    .Instance)
                .Given(argList = new ConstructorArgList(null, argDef1))

                .Then(argList.Arguments, Is(AList.InOrder().WithOnly(AnInstance.SameAs(arg1))));
        }

        [Test]
        public void RepeatedInjectableArg()
        {
            ConstructorArgList argList;
            IInjectableConstructorArgDefinition argDef1;
            IConstructorArg arg1;

            Scenario()
                .Given(arg1 = AMock<IConstructorArg>().Instance)
                .Given(argDef1 = AMock<IInjectableConstructorArgDefinition>()
                    .WhereMethod(d => d.FindExisting(ArgIsAny<IList<IConstructorArg>>())).ReturnsInSequence(null, arg1)
                    .WhereMethod(d => d.Define(ArgIsAny<TypeBuilder>())).Returns(arg1)
                    .Instance)
                .Given(argList = new ConstructorArgList(null, argDef1, argDef1))

                .Then(argList.Arguments, Is(AList.InOrder().WithOnly(AnInstance.SameAs(arg1))));
        }
    }
}
