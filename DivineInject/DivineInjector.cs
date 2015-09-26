using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivineInject
{
    public interface IBindingBuilder<TInterface>
    {
        IDivineInjector To<TImpl>()
                where TImpl : class;
    }

    public interface IDivineInjector
    {
        IBindingBuilder<T> Bind<T>();
        T Get<T>();
        object Get(Type type);
        bool IsBound(Type type);
    }

    class DivineInjector : IDivineInjector
    {
        private Instantiator m_instantiator;
        private IDictionary<Type, object> m_bindings = new Dictionary<Type, object>();

        public DivineInjector()
        {
            m_instantiator = new Instantiator(this);
        }

        public IBindingBuilder<T> Bind<T>()
        {
            return new BindingBuilder<T>(this);
        }

        public T Get<T>()
        {
            return (T) Get(typeof(T));
        }

        public object Get(Type type)
        {
            object impl;
            if (!m_bindings.TryGetValue(type, out impl))
                return Activator.CreateInstance(type);
            return impl;
        }

        public bool IsBound(Type type)
        {
            return m_bindings.ContainsKey(type);
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

            public IDivineInjector To<TImpl>()
                where TImpl : class
            {
                m_injector.AddBinding<TInterface, TImpl>();
                return m_injector;
            }
        }
    }

}
