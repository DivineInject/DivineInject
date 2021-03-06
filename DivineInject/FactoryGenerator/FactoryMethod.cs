using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DivineInject.FactoryGenerator
{
    internal class FactoryMethod : IFactoryMethod
    {
        public FactoryMethod(ConstructorInfo constructor, string name,
            Type returnType, Type returnImplType, Type[] parameterTypes, IList<IConstructorArgDefinition> constructorArgs)
        {
            Constructor = constructor;
            Name = name;
            ReturnType = returnType;
            ReturnImplType = returnImplType;
            ParameterTypes = parameterTypes;
            ConstructorArgs = constructorArgs;
        }

        public ConstructorInfo Constructor { get; private set; }
        public string Name { get; private set; }
        public Type ReturnType { get; private set; }
        public Type ReturnImplType { get; private set; }
        public Type[] ParameterTypes { get; private set; }
        public IList<IConstructorArgDefinition> ConstructorArgs { get; private set; }

        public void EmitMethod(TypeBuilder tb, IConstructorArgList constructorArgList)
        {
            var methodAttributes = MethodAttributes.Public |
                                   MethodAttributes.HideBySig |
                                   MethodAttributes.NewSlot |
                                   MethodAttributes.Virtual |
                                   MethodAttributes.Final;
            var method = tb.DefineMethod(
                Name,
                methodAttributes,
                CallingConventions.Standard,
                ReturnType,
                ConstructorArgs.OfType<IPassedConstructorArgDefinition>().Select(a => a.ParameterType).ToArray());

            var consArgTypes = ConstructorArgs.Select(a => a.ParameterType).ToArray();

            var conObj = ReturnImplType.GetConstructor(consArgTypes);

            if (conObj == null)
                throw new Exception("Failed to find constructor of type " + ReturnImplType.FullName + " with arguments: " + string.Join(", ", consArgTypes.Select(a => a.FullName)));

            ILGenerator il = method.GetILGenerator();
            il.DeclareLocal(ReturnImplType);

            il.Emit(OpCodes.Nop);
            foreach (var arg in ConstructorArgs)
            {
                if (arg is IInjectableConstructorArgDefinition)
                {
                    var passedArgument = constructorArgList.FindExisting(arg);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Call, ((IInjectableConstructorArg)passedArgument).Getter);
                }
                else if (arg is IPassedConstructorArgDefinition)
                {
                    il.Emit(OpCodes.Ldarg, ((IPassedConstructorArgDefinition)arg).ParameterIndex + 1);
                }
                else
                {
                    throw new Exception("Unrecognised type of argument: " + arg.GetType().FullName);
                }
            }

            il.Emit(OpCodes.Newobj, conObj);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);
        }
    }
}
