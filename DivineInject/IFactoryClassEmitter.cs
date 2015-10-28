using System.Collections.Generic;

namespace DivineInject
{
    public interface IFactoryClassEmitter
    {
        IList<InjectableConstructorArg> EmitInjectableProperties(IList<InjectableConstructorArgDefinition> definitions);
        void EmitConstructor(IList<InjectableConstructorArg> properties);
    }
}
