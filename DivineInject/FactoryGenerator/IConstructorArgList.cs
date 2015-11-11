using System.Collections.Generic;

namespace DivineInject.FactoryGenerator
{
    internal interface IConstructorArgList
    {
        void Add(IConstructorArgDefinition defn);
        IList<IConstructorArg> Arguments { get; }
        IConstructorArg FindExisting(IConstructorArgDefinition defn);
    }
}