using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace DotNetMerchant.Storefront.Configuration.Unity
{
    public class UnityDependencyContainer : IDependencyContainer
    {
        private readonly UnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityDependencyContainer"/> class.
        /// </summary>
        public UnityDependencyContainer()
        {
            _container = new UnityContainer();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.ResolveAll<T>();
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
    }
}