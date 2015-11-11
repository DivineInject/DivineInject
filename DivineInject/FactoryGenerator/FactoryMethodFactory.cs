using System;
using System.Linq;
using System.Reflection;

namespace DivineInject.FactoryGenerator
{
    internal class FactoryMethodFactory : IFactoryMethodFactory
    {
        public IFactoryMethod Create(MethodInfo method, IDivineInjector injector, Type domainObjectType)
        {
            var methodArgs = method.GetParameters();
            var constructors = domainObjectType.GetConstructors()
                .Where(cons => ConstructorCanBeCalled(cons, methodArgs, injector))
                .ToList();
            if (constructors.Count() > 1)
                throw new Exception("Multiple callable constructors found in target type " + domainObjectType);
            var constructor = constructors.SingleOrDefault();
            if (constructor == null)
                throw new Exception(
                    string.Format(
                        "Could not find constructor on {0} for factory method {1}.{2}",
                        domainObjectType.Name,
                        method.DeclaringType.Name,
                        method.Name));

            var consArgs = constructor.GetParameters()
                .Select(param => ToConstructorArg(method, param, injector))
                .ToList();

            return new FactoryMethod(constructor, method.Name, method.ReturnType, domainObjectType, methodArgs.Select(a => a.ParameterType).ToArray(), consArgs);
        }

        private IConstructorArgDefinition ToConstructorArg(MethodInfo method, ParameterInfo param, IDivineInjector injector)
        {
            if (injector.IsBound(param.ParameterType))
                return new InjectableConstructorArgDefinition(param.ParameterType, GetPropertyName(param.Name));
            return new PassedConstructorArgDefinition(param.ParameterType, MethodArgIndex(param, method));
        }

        private int MethodArgIndex(ParameterInfo param, MethodInfo method)
        {
            var matchingArgsOfCorrectType = method.GetParameters().Where(p => p.ParameterType == param.ParameterType).ToList();
            if (!matchingArgsOfCorrectType.Any())
                throw new Exception("Failed to match constructor arg with arg in method");
            if (matchingArgsOfCorrectType.Count == 1)
                return matchingArgsOfCorrectType.First().Position;
            var firstByName = matchingArgsOfCorrectType.FirstOrDefault(p => p.Name == param.Name);
            if (firstByName != null)
                return firstByName.Position;
            throw new Exception(string.Format("Failed to wire up parameter in method named {0} because there are multiple arguments of the same type and none of the constructor arguments have the same name", param.Name));
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
