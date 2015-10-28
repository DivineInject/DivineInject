using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DivineInject
{
    internal class FactoryClassEmitter : IFactoryClassEmitter
    {
        private readonly TypeBuilder m_tb;

        public FactoryClassEmitter(Type interfaceType)
        {
            m_tb = GetTypeBuilder(interfaceType);    
        }

        public IList<InjectableConstructorArg> EmitInjectableProperties(IList<InjectableConstructorArgDefinition> definitions)
        {
            return definitions.Select(d => d.CreateProperty(m_tb)).ToList();
        }

        public void EmitConstructor(IList<InjectableConstructorArg> properties)
        {
            var constructor = m_tb.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                properties.Select(p => p.PropertyType).ToArray());

            var conObj = typeof(object).GetConstructor(new Type[0]);

            ILGenerator il = constructor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, conObj);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Nop);

            for (var i = 0; i < properties.Count; i++)
            {
                var property = properties[i];

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg, i + 1);
                il.Emit(OpCodes.Call, property.Setter);
                il.Emit(OpCodes.Nop);
            }

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);
        }

        private static TypeBuilder GetTypeBuilder(Type interfaceType)
        {
            var typeSignature = "MyDynamicType";
            var an = new AssemblyName(typeSignature);
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            var tb = moduleBuilder.DefineType(typeSignature
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