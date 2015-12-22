using System;
using System.Linq;

namespace DivineInject
{
    internal class Instantiator
    {
        private readonly IDivineInjector m_injector;

        public Instantiator(IDivineInjector injector)
        {
            m_injector = injector;
        }

        public T Create<T>()
            where T : class
        {
            return (T) Create(typeof (T));
        }

        public object Create(Type type)
        {
            var constructors = type.GetConstructors();
            var cons = constructors
                .Where(c => c.GetParameters().All(p => m_injector.IsBound(p.ParameterType)))
                .OrderBy(c => c.GetParameters().Count())
                .FirstOrDefault();
            if (cons == null)
            {
                var constructorParameters = string.Join(
                    ", ", 
                    constructors
                        .SelectMany(c => c.GetParameters())
                        .Where(p => !m_injector.IsBound(p.ParameterType))
                        .Select(p => p.ParameterType.FullName));
                throw new BindingException(string.Format("Cannot create {0}, could not find an injectable constructor because the following types are not injectable: {1}",
                    type.FullName,
                    constructorParameters));
            }
            var args = cons.GetParameters().Select(p => m_injector.Get(p.ParameterType)).ToArray();
            return cons.Invoke(args);
        }
    }
}
