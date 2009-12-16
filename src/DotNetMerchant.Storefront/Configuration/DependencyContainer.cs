using System;
using System.Collections.Generic;

namespace DotNetMerchant.Storefront.Configuration
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly IDependencyContainer _container;

        public DependencyContainer(IDependencyContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public void RegisterInstance<T>(string name, T instance)
        {
            _container.RegisterInstance(name, instance);
        }

        public void RegisterInstance<T>(T instance)
        {
            _container.RegisterInstance(instance);
        }

        public void RegisterType<T, K>() where K : T
        {
            _container.RegisterType<T, K>();
        }

        public void RegisterType(Type contract, Type concrete)
        {
            _container.RegisterType(contract, concrete);
        }

        public void RegisterType(Type contract, Type concrete, string name)
        {
            _container.RegisterType(contract, concrete, name);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.ResolveAll<T>();
        }
    }
}