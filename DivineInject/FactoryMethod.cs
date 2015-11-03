using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DivineInject
{
    public interface IFactoryMethod
    {
        ConstructorInfo Constructor { get; }
        string Name { get; }
        Type ReturnType { get; }
        Type ReturnImplType { get; }
        Type[] ParameterTypes { get; }
        IList<IConstructorArgDefinition> ConstructorArgs { get; }
        void EmitMethod(TypeBuilder tb, IConstructorArgList constructorArgList);
    }

    internal class FactoryMethod : IFactoryMethod
    {
        public ConstructorInfo Constructor { get; private set; }
        public string Name { get; private set; }
        public Type ReturnType { get; private set; }
        public Type ReturnImplType { get; private set; }
        public Type[] ParameterTypes { get; private set; }
        public IList<IConstructorArgDefinition> ConstructorArgs { get; private set; } 

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

        public void EmitMethod(TypeBuilder tb, IConstructorArgList constructorArgList)
        {
            var method = tb.DefineMethod(Name,
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.NewSlot |
                MethodAttributes.Virtual |
                MethodAttributes.Final,
                CallingConventions.Standard,
                ReturnType,
                ConstructorArgs.Select(a => a.ParameterType).ToArray()
                );

            var consArgs = ConstructorArgs
                .OfType<IPassedConstructorArgDefinition>()
                .Select(d => d.ParameterType)
                .ToArray();

            var conObj = ReturnImplType.GetConstructor(consArgs);

            if (conObj == null)
                throw new Exception("Failed to find constructor of type " + ReturnImplType.FullName + " with arguments: " + string.Join(", ", consArgs.Select(a => a.FullName)));

            ILGenerator il = method.GetILGenerator();
            il.DeclareLocal(ReturnImplType);

            il.Emit(OpCodes.Nop);
            foreach (var arg in ConstructorArgs)
            {
                var passedArgument = constructorArgList.FindExisting(arg);
                if (passedArgument is IInjectableConstructorArg)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Call, ((IInjectableConstructorArg)passedArgument).Getter);
                }
                else if (passedArgument is IPassedConstructorArg)
                {
                    il.Emit(OpCodes.Ldarg, ((IPassedConstructorArg) passedArgument).ParameterIndex + 1);
                }
                else
                {
                    throw new Exception("Unrecognised type of argument: " + passedArgument.GetType().FullName);
                }
            }

            il.Emit(OpCodes.Newobj, conObj);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);
        }
    }
}
