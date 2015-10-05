﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DivineInject
{
    class ClassGenerator
    {
        public TInterface Generate<TInterface, TImpl>(IList<GeneratedProperty> properties, IList<ConstructorArg> constructorArgs)
        {
            return (TInterface) CreateNewObject(properties, constructorArgs, typeof(TInterface), typeof(TImpl));
        }

        private static object CreateNewObject(IList<GeneratedProperty> properties, IList<ConstructorArg> constructorArgs, Type interfaceType, Type implType)
        {
            var myType = CompileResultType(properties, constructorArgs, interfaceType, implType);
            return Activator.CreateInstance(myType, properties.Select(p => p.PropertyValue).ToArray());
        }

        public static Type CompileResultType(IList<GeneratedProperty> properties, IList<ConstructorArg> constructorArgs, Type interfaceType, Type implType)
        {
            TypeBuilder tb = GetTypeBuilder(interfaceType);

            foreach (var property in properties)
                CreateProperty(tb, property);

            CreateConstructor(tb, properties);

            foreach (var method in interfaceType.GetMethods())
                CreateMethod(tb, method, implType, properties, constructorArgs);

            Type objectType = tb.CreateType();
            return objectType;
        }

        private static void CreateMethod(TypeBuilder tb, MethodInfo methodInfo, Type implType, IList<GeneratedProperty> properties, IList<ConstructorArg> constructorArgs)
        {
            var method = tb.DefineMethod(methodInfo.Name,
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.NewSlot |
                MethodAttributes.Virtual |
                MethodAttributes.Final,
                CallingConventions.Standard,
                methodInfo.ReturnType,
                new Type[0]
                );

            var conObj = implType.GetConstructor(constructorArgs.Select(c => c.ArgType).ToArray());

            if (conObj == null)
                throw new Exception("Failed to find constructor");

            ILGenerator il = method.GetILGenerator();
            il.DeclareLocal(methodInfo.ReturnType);

            il.Emit(OpCodes.Nop);
            foreach (var arg in constructorArgs)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, properties[arg.PropertyIndex].Getter);
            }

            il.Emit(OpCodes.Newobj, conObj);
            
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);
        }

        private static void CreateConstructor(TypeBuilder tb, IList<GeneratedProperty> properties)
        {
            var constructor = tb.DefineConstructor(
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
                il.Emit(OpCodes.Ldarg, i+1);
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

        private static void CreateProperty(TypeBuilder tb, GeneratedProperty property)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + property.Name, property.PropertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(property.Name, PropertyAttributes.HasDefault, property.PropertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + property.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, 
                property.PropertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + property.Name,
                  MethodAttributes.Private |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { property.PropertyType });

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

            property.Getter = getPropMthdBldr;
            property.Setter = setPropMthdBldr;
        }
    }
}
