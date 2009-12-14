using System;
using System.Collections.Generic;

namespace DotNetMerchant.Storefront.Configuration
{
    public interface IDependencyContainer
    {
        IEnumerable<T> ResolveAll<T>();
        T Resolve<T>();
        object Resolve(Type type);
        void RegisterInstance<T>(string name, T instance);
        void RegisterInstance<T>(T instance);
        void RegisterType<T, K>() where K : T;
    }
}