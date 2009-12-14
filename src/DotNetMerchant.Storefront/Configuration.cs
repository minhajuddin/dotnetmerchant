using DotNetMerchant.Storefront.Configuration;
using DotNetMerchant.Storefront.Configuration.Unity;

namespace DotNetMerchant.Storefront
{
    public class DotNetMerchantConfiguration : IDependencyConfiguration
    {
        private IDependencyContainer _dependencyContainer;

        public void Configure()
        {
            var dependencyContainer = new UnityDependencyContainer();
            _dependencyContainer = new DependencyContainer(dependencyContainer);

            // VPP

            // Embedded Views

            // Bootstrapper Tasks

            // Services

            // Filters
        }
    }
}