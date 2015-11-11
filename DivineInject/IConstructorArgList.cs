using System.Collections.Generic;

namespace DivineInject
{
    public interface IConstructorArgList
    {
        void Add(IConstructorArgDefinition defn);
        IList<IConstructorArg> Arguments { get; }
        IConstructorArg FindExisting(IConstructorArgDefinition defn);
    }
}