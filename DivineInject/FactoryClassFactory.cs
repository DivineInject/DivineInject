using System;
using System.Linq;

namespace DivineInject
{
    internal class FactoryClassFactory
    {
        private readonly IConstructorArgList m_consArgList;
        private readonly IFactoryMethodFactory m_methodFactory;

        public FactoryClassFactory(IConstructorArgList consArgList, IFactoryMethodFactory methodFactory)
        {
            m_consArgList = consArgList;
            m_methodFactory = methodFactory;
        }

        public FactoryClass Create(Type factoryInterface, IDivineInjector injector, Type domainObjectType)
        {
            var methods = factoryInterface.GetMethods()
                .Select(m => m_methodFactory.Create(m, injector, domainObjectType))
                .ToList();
            return new FactoryClass(m_consArgList, methods);
        }
    }
}
