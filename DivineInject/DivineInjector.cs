using System;
using System.Collections.Generic;
using DivineInject.FactoryGenerator;

namespace DivineInject
{
    public interface IBindingBuilder
    {
        IDivineInjector To<TImpl>()
            where TImpl : class;

        IDivineInjector To(Type implType);

        IDivineInjector ToInstance<TImpl>(TImpl instance)
            where TImpl : class;

        IDivineInjector AsGeneratedFactoryFor<TImpl>()
            where TImpl : class;
    }

    public interface IDivineInjector
    {
        IBindingBuilder Bind<T>();
        IBindingBuilder Bind(Type type);
        T Get<T>();
        object Get(Type type);
        bool IsBound(Type type);
        void Reset();
    }

    public class DivineInjector : IDivineInjector
    {
        private static readonly IDivineInjector CurrentInstance = new DivineInjector();
        private readonly Instantiator m_instantiator;
        private readonly IDictionary<Type, object> m_bindings = new Dictionary<Type, object>();

        private DivineInjector()
        {
            m_instantiator = new Instantiator(this);
        }

        public static IDivineInjector Current
        {
            get { return CurrentInstance; }
        }

        public IBindingBuilder Bind<T>()
        {
            return new BindingBuilder(this, typeof(T));
        }

        public IBindingBuilder Bind(Type type)
        {
            return new BindingBuilder(this, type);
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

        public void Reset()
        {
            m_bindings.Clear();
        }

        private void AddBinding(Type interfaceType, Type implType)
        {
            var impl = m_instantiator.Create(implType);
            m_bindings.Add(interfaceType, impl);
        }

        private void AddBinding<TImpl>(Type interfaceType, TImpl instance)
            where TImpl : class
        {
            m_bindings.Add(interfaceType, instance);
        }

        private void AddFactoryBinding(Type interfaceType, Type implType)
        {
            var emitter = new FactoryClassEmitter(this, interfaceType, implType);
            var factory = emitter.CreateNewObject();
            m_bindings.Add(interfaceType, factory);
        }

        private class BindingBuilder : IBindingBuilder
        {
            private readonly DivineInjector m_injector;
            private readonly Type m_interfaceType;

            internal BindingBuilder(DivineInjector injector, Type interfaceType)
            {
                m_injector = injector;
                m_interfaceType = interfaceType;
            }

            public IDivineInjector To<TImpl>()
                where TImpl : class
            {
                return To(typeof (TImpl));
            }

            public IDivineInjector To(Type implType)
            {
                m_injector.AddBinding(m_interfaceType, implType);
                return m_injector;
            }

            public IDivineInjector ToInstance<TImpl>(TImpl instance)
                where TImpl : class
            {
                m_injector.AddBinding(m_interfaceType, instance);
                return m_injector;
            }

            public IDivineInjector AsGeneratedFactoryFor<TImpl>() where TImpl : class
            {
                m_injector.AddFactoryBinding(m_interfaceType, typeof(TImpl));
                return m_injector;
            }
        }
    }
}
