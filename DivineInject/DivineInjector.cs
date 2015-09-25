using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivineInject
{
    public interface IBindingBuilder<TInterface>
    {
        DivineInjector To<TImpl>()
                where TImpl : class;
    }

    public class DivineInjector
    {
        private Instantiator m_instantiator = new Instantiator();
        private IDictionary<Type, object> m_bindings = new Dictionary<Type, object>();

        public IBindingBuilder<T> Bind<T>()
        {
            return new BindingBuilder<T>(this);
        }

        public T Get<T>()
        {
            object impl;
            if (!m_bindings.TryGetValue(typeof(T), out impl))
                return default(T);
            return (T) impl;
        }

        private void AddBinding<TInterface, TImpl>()
            where TImpl : class
        {
            var impl = m_instantiator.Create<TImpl>();
            m_bindings.Add(typeof(TInterface), impl);
        }

        private class BindingBuilder<TInterface> : IBindingBuilder<TInterface>
        {
            private DivineInjector m_injector;

            internal BindingBuilder(DivineInjector injector)
            {
                m_injector = injector;
            }

            public DivineInjector To<TImpl>()
                where TImpl : class
            {
                m_injector.AddBinding<TInterface, TImpl>();
                return m_injector;
            }
        }
    }

}
