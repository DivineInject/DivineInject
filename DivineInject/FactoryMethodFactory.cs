using System;
using System.Linq;
using System.Reflection;

namespace DivineInject
{
    public interface IFactoryMethodFactory
    {
        IFactoryMethod Create(MethodInfo method, IDivineInjector injector, Type domainObjectType);
    }

    internal class FactoryMethodFactory : IFactoryMethodFactory
    {
        public IFactoryMethod Create(MethodInfo method, IDivineInjector injector, Type domainObjectType)
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

            var consArgs = constructor.GetParameters()
                .Select(param => ToConstructorArg(method, param, injector))
                .ToList();

            return new FactoryMethod(constructor, method.Name, method.ReturnType, consArgs);
        }

        private IConstructorArgDefinition ToConstructorArg(MethodInfo method, ParameterInfo param, IDivineInjector injector)
        {
            if (injector.IsBound(param.ParameterType))
                return new InjectableConstructorArgDefinition(param.ParameterType, GetPropertyName(param.Name));
            return new PassedConstructorArgDefinition(param.ParameterType, MethodArgIndex(param, method));
        }

        private int MethodArgIndex(ParameterInfo param, MethodInfo method)
        {
            var matchingArgInMethod = method.GetParameters().FirstOrDefault(p => p.ParameterType == param.ParameterType);
            if (matchingArgInMethod == null)
                throw new Exception("Failed to match constructor arg with arg in method");
            return matchingArgInMethod.Position;
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
