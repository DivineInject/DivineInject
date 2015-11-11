using System.Collections.Generic;

namespace DivineInject.FactoryGenerator
{
    internal interface IConstructorArgList
    {
        IList<IConstructorArg> Arguments { get; }
        void Add(IConstructorArgDefinition defn);
        IConstructorArg FindExisting(IConstructorArgDefinition defn);
    }
}