using System;
using System.Reflection;
using System.Web;

namespace DotNetMerchant.Storefront.Configuration
{
    public class Bootstrapper<T> where T : IDependencyConfiguration
    {
        public static void Run()
        {
            var assembly = Assembly.GetCallingAssembly();

            Activator.CreateInstance<T>().Configure(assembly);

            var dependencyContainer =
                HttpContext.Current.Application[Globals.DependencyContainer]
                as IDependencyContainer;

            if (dependencyContainer == null)
            {
                return;
            }

            var tasks = dependencyContainer.ResolveAll<IBootstrapperTask>();
            foreach (var task in tasks)
            {
                task.Execute();
            }
        }
    }
}