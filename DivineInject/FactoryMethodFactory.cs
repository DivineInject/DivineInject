using System;
using System.Linq;
using System.Reflection;

namespace DivineInject
{
    internal class FactoryMethodFactory
    {
        public FactoryMethod Create(MethodInfo method, IDivineInjector injector, Type domainObjectType)
        {
            var constructors = domainObjectType.GetConstructors();

            return new FactoryMethod(constructors.First(), new GeneratedProperty[0]);
        }
    }
}
