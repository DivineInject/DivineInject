﻿using System;
using System.Linq;
using System.Reflection;

namespace DivineInject
{
    internal class FactoryMethodFactory
    {
        public FactoryMethod Create(MethodInfo method, IDivineInjector injector, Type domainObjectType)
        {
            var methodArgs = method.GetParameters();
            var constructors = domainObjectType.GetConstructors().Where(cons => ConstructorCanBeCalled(cons, methodArgs, injector));
            var constructor = constructors.SingleOrDefault();
            if (constructor == null)
                throw new Exception(
                    string.Format(
                        "Could not find constructor on {0} for factory method {1}.{2}",
                        domainObjectType.Name,
                        method.DeclaringType.Name,
                        method.Name));

            var properties = constructor.GetParameters()
                .Where(param => injector.IsBound(param.ParameterType))
                .Select(param => new GeneratedProperty(param.ParameterType, GetPropertyName(param.Name), injector.Get(param.ParameterType)))
                .ToList();

            return new FactoryMethod(constructor, properties);
        }

        private string GetPropertyName(string name)
        {
            return char.ToUpper(name[0]) + name.Substring(1);
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
