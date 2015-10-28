using System;

namespace DivineInject.Test.DummyModel
{
    internal class DomainObjectWithSingleArgConstructor : IDomainObject
    {
        public DomainObjectWithSingleArgConstructor()
        {
            throw new NotImplementedException();
        }

        public DomainObjectWithSingleArgConstructor(string name)
        {
            
        }
    }
}