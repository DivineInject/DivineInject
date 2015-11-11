using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DivineInject.FactoryGenerator
{
    internal class InjectableConstructorArgDefinition : IInjectableConstructorArgDefinition
    {
        public InjectableConstructorArgDefinition(Type parameterType, string name)
        {
            ParameterType = parameterType;
            Name = name;
        }

        public Type ParameterType { get; private set; }
        public string Name { get; private set; }

        public IConstructorArg Define(TypeBuilder tb)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + Name, ParameterType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(Name, PropertyAttributes.HasDefault, ParameterType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                ParameterType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            var methodAttributes = MethodAttributes.Private |
                                   MethodAttributes.SpecialName |
                                   MethodAttributes.HideBySig;
            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + Name,
                    methodAttributes,
                    null, new[] { ParameterType });

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

            return new InjectableConstructorArg(ParameterType, Name, getPropMthdBldr, setPropMthdBldr);
        }

        public IConstructorArg FindExisting(IList<IConstructorArg> arguments)
        {
            return arguments.OfType<IInjectableConstructorArg>()
                .FirstOrDefault(a => a.Name == Name && a.PropertyType == ParameterType);
        }
    }
}
