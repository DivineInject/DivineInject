using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivineInject
{
    public interface IBindingBuilder<TInterface>
    {
        DivineInjector To<TImpl>();
    }

    public class DivineInjector
    {
        private IDictionary<Type, Type> m_bindings = new Dictionary<Type, Type>();

        public IBindingBuilder<T> Bind<T>()
        {
            return new BindingBuilder<T>(this);
        }

        public T Get<T>()
        {
            Type implType;
            if (!m_bindings.TryGetValue(typeof(T), out implType))
                return default(T);
            return (T) Activator.CreateInstance(implType);
        }

        private void AddBinding<TInterface, TImpl>()
        {
            m_bindings.Add(typeof(TInterface), typeof(TImpl));
        }

        private class BindingBuilder<TInterface> : IBindingBuilder<TInterface>
        {
            private DivineInjector m_injector;

            internal BindingBuilder(DivineInjector injector)
            {
                m_injector = injector;
            }

            public DivineInjector To<TImpl>()
            {
                m_injector.AddBinding<TInterface, TImpl>();
                return m_injector;
            }
        }
    }

}
