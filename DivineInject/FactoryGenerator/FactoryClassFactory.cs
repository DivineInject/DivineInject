using System;
using System.Linq;

namespace DivineInject.FactoryGenerator
{
    internal class FactoryClassFactory
    {
        private readonly IFactoryMethodFactory m_methodFactory;

        public FactoryClassFactory(IFactoryMethodFactory methodFactory)
        {
            m_methodFactory = methodFactory;
        }

        public FactoryClass Create(Type factoryInterface, IDivineInjector injector, Type domainObjectType, ConstructorArgList constructorArgList)
        {
            var methods = factoryInterface.GetMethods()
                .Select(m => m_methodFactory.Create(m, injector, domainObjectType))
                .ToList();
            return new FactoryClass(constructorArgList, methods);
        }
    }
}
