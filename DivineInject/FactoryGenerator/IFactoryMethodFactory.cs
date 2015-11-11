using System;
using System.Reflection;

namespace DivineInject.FactoryGenerator
{
    internal interface IFactoryMethodFactory
    {
        IFactoryMethod Create(MethodInfo method, IDivineInjector injector, Type domainObjectType);
    }
}