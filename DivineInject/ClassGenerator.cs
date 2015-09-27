using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace DivineInject
{
    class ClassGenerator
    {
        public object Generate<TInterface, TImpl>(IList<GeneratedProperty> properties)
        {
            return CreateNewObject(properties, typeof(TInterface), typeof(TImpl));
        }

        private static object CreateNewObject(IList<GeneratedProperty> properties, Type interfaceType, Type implType)
        {
            var myType = CompileResultType(properties, interfaceType, implType);
            return Activator.CreateInstance(myType, properties.Select(p => p.PropertyValue).ToArray());
        }

        public static Type CompileResultType(IList<GeneratedProperty> properties, Type interfaceType, Type implType)
        {
            TypeBuilder tb = GetTypeBuilder(interfaceType);

            var setters = properties.Select(property => CreateProperty(tb, property.Name, property.PropertyType)).ToList();

            CreateConstructor(tb, properties, setters);

            foreach (var method in interfaceType.GetMethods())
                CreateMethod(tb, method, implType);

            Type objectType = tb.CreateType();
            return objectType;
        }

        private static void CreateMethod(TypeBuilder tb, MethodInfo methodInfo, Type implType)
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

            var conObj = implType.GetConstructor(new Type[0]);

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, conObj);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);
        }

        private static void CreateConstructor(TypeBuilder tb, IList<GeneratedProperty> properties, IList<MethodBuilder> setters)
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
                var setter = setters[i];

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg, i+1);
                il.Emit(OpCodes.Call, setter);
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

        private static MethodBuilder CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Private |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

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

            return setPropMthdBldr;
        }
    }
}
