using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetMerchant.Storefront.Extensions
{
    internal static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetInterfaces(this Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsInterface);
        }

        public static IEnumerable<Type> GetConcreteTypesFor(this Assembly assembly, Type interfaceType)
        {
            var concreteTypes = assembly.GetTypes().Where(t =>
                !t.IsInterface && !t.IsAbstract && interfaceType.IsAssignableFrom(t)
            );

            foreach(var concreteType in concreteTypes)
            {
                yield return concreteType;   
            }
        }
    }
}
