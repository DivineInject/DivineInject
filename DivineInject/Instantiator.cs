using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivineInject
{
    internal class Instantiator
    {
        private IDivineInjector m_injector;

        public Instantiator(IDivineInjector injector)
        {
            m_injector = injector;
        }

        public T Create<T>()
            where T : class
        {
            var constructors = typeof(T).GetConstructors();
            var cons = constructors.First();
            var args = cons.GetParameters().Select(p => m_injector.Get(p.ParameterType)).ToArray();
            return (T) cons.Invoke(args);
        }
    }
}
