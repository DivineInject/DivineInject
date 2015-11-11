using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DivineInject.FactoryGenerator
{
    internal class FactoryClassEmitter 
    {
        private readonly Type m_interfaceType;
        private readonly IDivineInjector m_injector;
        private readonly Type m_domainObjectType;
        private readonly TypeBuilder m_tb;
        private readonly FactoryClassFactory m_factoryClassFactory;

        public FactoryClassEmitter(IDivineInjector injector, Type interfaceType, Type domainObjectType)
        {
            m_interfaceType = interfaceType;
            m_injector = injector;
            m_domainObjectType = domainObjectType;
            m_tb = GetTypeBuilder(interfaceType);
            m_factoryClassFactory = new FactoryClassFactory(new FactoryMethodFactory());
        }

        public object CreateNewObject()
        {
            var constructorArgList = new ConstructorArgList(m_tb);
            var myType = CompileResultType(constructorArgList);
            var propertyValues = constructorArgList.Arguments
                .OfType<IInjectableConstructorArg>()
                .Select(a => m_injector.Get(a.PropertyType))
                .ToArray();
            return Activator.CreateInstance(myType, propertyValues);
        }

        private static TypeBuilder GetTypeBuilder(Type interfaceType)
        {
            var typeSignature = interfaceType.Name + "Factory";
            var an = new AssemblyName("Divine.Inject.Generated");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            var typeAttributes = TypeAttributes.Public |
                                 TypeAttributes.Class |
                                 TypeAttributes.AutoClass |
                                 TypeAttributes.AnsiClass |
                                 TypeAttributes.BeforeFieldInit |
                                 TypeAttributes.AutoLayout;
            var tb = moduleBuilder.DefineType(
                typeSignature, 
                typeAttributes, 
                null);
            tb.AddInterfaceImplementation(interfaceType);
            return tb;
        }

        private Type CompileResultType(ConstructorArgList constructorArgList)
        {
            var factoryClass = m_factoryClassFactory.Create(m_interfaceType, m_injector, m_domainObjectType, constructorArgList);

            factoryClass.EmitConstructor(m_tb);

            foreach (var method in factoryClass.Methods)
                method.EmitMethod(m_tb, constructorArgList);

            return m_tb.CreateType();
        }
    }
}