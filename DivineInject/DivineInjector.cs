using System;
using System.Collections.Generic;
using DivineInject.FactoryGenerator;

namespace DivineInject
{
    public interface IBindingBuilder
    {
        IDivineInjector To<TImpl>()
            where TImpl : class;

        IDivineInjector AsGeneratedFactoryFor<TImpl>()
            where TImpl : class;
    }

    public interface IDivineInjector
    {
        IBindingBuilder Bind<T>();
        T Get<T>();
        object Get(Type type);
        bool IsBound(Type type);
    }

    public class DivineInjector : IDivineInjector
    {
        private static readonly IDivineInjector CurrentInstance = new DivineInjector();
        private readonly Instantiator m_instantiator;
        private readonly IDictionary<Type, object> m_bindings = new Dictionary<Type, object>();

        public DivineInjector()
        {
            m_instantiator = new Instantiator(this);
        }

        public static IDivineInjector Current
        {
            get { return CurrentInstance; }
        }

        public IBindingBuilder Bind<T>()
        {
            return new BindingBuilder<T>(this);
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
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

        private void AddFactoryBinding<TInterface, TImpl>()
            where TImpl : class
        {
            var emitter = new FactoryClassEmitter(this, typeof(TInterface), typeof(TImpl));
            var factory = emitter.CreateNewObject();
            m_bindings.Add(typeof(TInterface), factory);
        }

        private class BindingBuilder<TInterface> : IBindingBuilder
        {
            private readonly DivineInjector m_injector;

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

            public IDivineInjector AsGeneratedFactoryFor<TImpl>() where TImpl : class
            {
                m_injector.AddFactoryBinding<TInterface, TImpl>();
                return m_injector;
            }
        }
    }
}
