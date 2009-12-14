using System.Web;
using System.Web.Routing;
using DotNetMerchant.Storefront.Configuration;

namespace DotNetMerchant.Storefront.Services
{
    public class PluginService : IPluginService
    {
        public RouteCollection GetRouteCollection()
        {
            var dependencyContainer = GetDependencyContainer();
            return dependencyContainer == null
                       ? null
                       : dependencyContainer.Resolve<RouteCollection>();
        }

        public IDependencyContainer GetDependencyContainer()
        {
            var dependencyContainer =
                HttpContext.Current.Application[Globals.DependencyContainer]
                as IDependencyContainer;

            return dependencyContainer;
        }
    }
}