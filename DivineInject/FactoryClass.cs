using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DivineInject
{
    internal class FactoryClass
    {
        private readonly IConstructorArgList m_argList;
        public IList<IFactoryMethod> Methods { get; private set; }

        public FactoryClass(IConstructorArgList argList, IList<IFactoryMethod> methods)
        {
            m_argList = argList;
            Methods = methods;
            foreach (var definition in methods.SelectMany(m => m.ConstructorArgs))
                argList.Add(definition);
        }

        public void EmitConstructor(TypeBuilder tb)
        {
            CreateConstructor(tb, m_argList.Arguments.OfType<IInjectableConstructorArg>().ToList());
        }

        private static void CreateConstructor(TypeBuilder tb, IList<IInjectableConstructorArg> properties)
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
                il.Emit(OpCodes.Ldarg, i + 1);
                il.Emit(OpCodes.Call, property.Setter);
                il.Emit(OpCodes.Nop);
            }

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);
        }
    }
}