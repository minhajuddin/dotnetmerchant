using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotNetMerchant.Storefront.Configuration;
using DotNetMerchant.Storefront.Configuration.Unity;
using DotNetMerchant.Storefront.Factories;
using DotNetMerchant.Storefront.Tasks;

namespace DotNetMerchant.Storefront
{
    public class StorefrontConfiguration : IDependencyConfiguration
    {
        private IDependencyContainer _dependencyContainer;

        public void Configure()
        {
            var unityDependencyContainer = new UnityDependencyContainer();
            _dependencyContainer = new DependencyContainer(unityDependencyContainer);

            // VPP

            // Embedded Views

            // Bootstrapper Tasks
            _dependencyContainer.RegisterInstance<IBootstrapperTask>("RegisterRoutes", new RegisterRoutesTask());

            // Services
            _dependencyContainer.RegisterInstance(RouteTable.Routes);
            
            // Filters


            // Factories
            var factory = new DependencyContainerControllerFactory(_dependencyContainer);
            ControllerBuilder.Current.SetControllerFactory(factory);           

            // Self-registration
            _dependencyContainer.RegisterInstance(_dependencyContainer);
            HttpContext.Current.Application[Globals.DependencyContainer] = _dependencyContainer;
        }
    }
}