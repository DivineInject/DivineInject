using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DivineInject
{
    internal class FactoryMethodFactory
    {
        public FactoryMethod Create(MethodInfo method, IDivineInjector injector, Type domainObjectType)
        {
            var methodArgs = method.GetParameters();
            var constructors = domainObjectType.GetConstructors()
                .Where(cons => ConstructorHasAllMethodArgs(cons, methodArgs));
            var constructor = constructors.Single();

            return new FactoryMethod(constructor, new GeneratedProperty[0]);
        }

        private bool ConstructorHasAllMethodArgs(ConstructorInfo cons, ParameterInfo[] methodArgs)
        {
            var consArgs = cons.GetParameters();

            var consArgTypes = consArgs.Select(a => a.ParameterType).ToList();
            var methodArgTypes = methodArgs.Select(a => a.ParameterType).ToList();

            return !consArgTypes.Except(methodArgTypes).Any() &&
                !methodArgTypes.Except(consArgTypes).Any();
        }
    }
}
