using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace DivineInject
{
    public interface IInjectableConstructorArgDefinition : IConstructorArgDefinition
    {
        Type PropertyType { get; }
        string Name { get; }
    }

    public class InjectableConstructorArgDefinition : IInjectableConstructorArgDefinition
    {
        public InjectableConstructorArgDefinition(Type propertyType, string name)
        {
            PropertyType = propertyType;
            Name = name;
        }

        public Type PropertyType { get; private set; }
        public string Name { get; private set; }

        public IConstructorArg Define(TypeBuilder tb)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + Name, PropertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(Name, PropertyAttributes.HasDefault, PropertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                PropertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + Name,
                    MethodAttributes.Private |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    null, new[] { PropertyType });

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

            return new InjectableConstructorArg(PropertyType, Name, getPropMthdBldr, setPropMthdBldr);
        }

        public IConstructorArg FindExisting(IList<IConstructorArg> arguments)
        {
            throw new NotImplementedException();
        }
    }
}
