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
                .Where(cons => ConstructorCanBeCalled(cons, methodArgs, injector));
            var constructor = constructors.Single();

            return new FactoryMethod(constructor, new GeneratedProperty[0]);
        }

        private bool ConstructorCanBeCalled(ConstructorInfo cons, ParameterInfo[] methodArgs, IDivineInjector injector)
        {
            var constructorParams = cons.GetParameters()
                .Where(param => !injector.IsBound(param.ParameterType))
                .ToArray();
            return ConstructorHasAllMethodArgs(methodArgs, constructorParams);
        }

        private bool ConstructorHasAllMethodArgs(ParameterInfo[] methodArgs, ParameterInfo[] constructorParams)
        {
            var consArgTypes = constructorParams.Select(a => a.ParameterType).ToList();
            var methodArgTypes = methodArgs.Select(a => a.ParameterType).ToList();

            return !consArgTypes.Except(methodArgTypes).Any() &&
                !methodArgTypes.Except(consArgTypes).Any();
        }
    }
}
