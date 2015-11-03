using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DivineInject
{
    internal class FactoryClass
    {
        public IList<IFactoryMethod> Methods { get; private set; }

        public FactoryClass(Type interfaceType, IList<IFactoryMethod> methods)
            : this(new ConstructorArgList(GetTypeBuilder(interfaceType)), methods)
        {
        }

        internal FactoryClass(IConstructorArgList argList, IList<IFactoryMethod> methods)
        {
            Methods = methods;
            foreach (var definition in methods.SelectMany(m => m.ConstructorArgs))
                argList.Add(definition);
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