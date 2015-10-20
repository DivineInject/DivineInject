using System;

namespace DivineInject
{
    internal class FactoryClassFactory
    {
        private readonly IFactoryMethodFactory m_methodFactory;

        public FactoryClassFactory(IFactoryMethodFactory methodFactory)
        {
            m_methodFactory = methodFactory;
        }

        public FactoryClass Create(Type factoryInterface, IDivineInjector injector, Type domainObjectType)
        {
            throw new NotImplementedException();
        }
    }
}
