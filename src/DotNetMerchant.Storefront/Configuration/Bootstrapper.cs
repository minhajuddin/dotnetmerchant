using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetMerchant.Storefront.Configuration
{
    public class Bootstrapper<T> where T : IDependencyConfiguration
    {
        public static void Run()
        {
            Activator.CreateInstance<T>().Configure();

            var dependencyContainer =
                HttpContext.Current.Application[Globals.DependencyContainer]
                as IDependencyContainer;

            if (dependencyContainer == null)
            {
                return;
            }

            var tasks = GetPendingTasks(dependencyContainer);
            while (tasks.Count() > 0)
            {
                foreach(var task in tasks)
                {
                    task.Execute();
                }

                tasks = GetPendingTasks(dependencyContainer);
            }
        }

        private static IEnumerable<IBootstrapperTask> GetPendingTasks(IDependencyContainer source)
        {
            return source.ResolveAll<IBootstrapperTask>().Where(t => !t.HasExecuted);
        }
    }
}