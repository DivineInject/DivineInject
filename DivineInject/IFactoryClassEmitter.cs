using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DivineInject
{
    public interface IFactoryClassEmitter
    {
        IList<InjectableConstructorArg> EmitInjectableProperties(IList<InjectableConstructorArgDefinition> definitions);
        void EmitConstructor(IList<InjectableConstructorArg> properties);
    }

    internal class FactoryClassEmitter : IFactoryClassEmitter
    {
        private readonly TypeBuilder m_tb;

        public FactoryClassEmitter(Type interfaceType)
        {
            m_tb = GetTypeBuilder(interfaceType);    
        }

        public IList<InjectableConstructorArg> EmitInjectableProperties(IList<InjectableConstructorArgDefinition> definitions)
        {
            return definitions.Select(d => CreateProperty(m_tb, d)).ToList();
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

        private static InjectableConstructorArg CreateProperty(TypeBuilder tb, InjectableConstructorArgDefinition definition)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + definition.Name, definition.PropertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(definition.Name, PropertyAttributes.HasDefault, definition.PropertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + definition.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                definition.PropertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + definition.Name,
                  MethodAttributes.Private |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { definition.PropertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);

            return new InjectableConstructorArg(definition.PropertyType, definition.Name, getPropMthdBldr, setPropMthdBldr);
        }
    }
}
