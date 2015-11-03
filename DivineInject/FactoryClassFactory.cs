using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

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
            var methods = factoryInterface.GetMethods()
                .Select(m => m_methodFactory.Create(m, injector, domainObjectType))
                .ToList();
            return new FactoryClass(GetTypeBuilder(factoryInterface), methods);
        }

        private static TypeBuilder GetTypeBuilder(Type interfaceType)
        {
            var typeSignature = "MyDynamicType";
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , null);
            tb.AddInterfaceImplementation(interfaceType);
            return tb;
        }
    }
}
