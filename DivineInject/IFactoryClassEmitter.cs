using System.Collections.Generic;

namespace DivineInject
{
    public interface IFactoryClassEmitter
    {
        IList<IConstructorArg> DefineArguments(IList<IConstructorArgDefinition> definitions);
        void EmitConstructor(IList<InjectableConstructorArg> properties);
    }
}
