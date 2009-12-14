using System.Web.Routing;
using DotNetMerchant.Storefront.Configuration;

namespace DotNetMerchant.Storefront.Services
{
    public interface IPluginService
    {
        IDependencyContainer GetDependencyContainer();
        RouteCollection GetRouteCollection();
    }
}