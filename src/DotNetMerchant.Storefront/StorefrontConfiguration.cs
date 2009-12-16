using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotNetMerchant.Storefront.Configuration;
using DotNetMerchant.Storefront.Configuration.Unity;
using DotNetMerchant.Storefront.Extensions;
using DotNetMerchant.Storefront.Factories;
using DotNetMerchant.Storefront.Tasks;

namespace DotNetMerchant.Storefront
{
    public class StorefrontConfiguration : IDependencyConfiguration
    {
        private IDependencyContainer _dependencyContainer;
        private Assembly _app;
        
        public void Configure(Assembly assembly)
        {
            _app = assembly;

            var unityDependencyContainer = new UnityDependencyContainer();
            _dependencyContainer = new DependencyContainer(unityDependencyContainer);

            // VPP

            // Embedded Views

            // Core Services
            RegisterCoreServices();

            // Other Services
            _dependencyContainer.RegisterInstance(RouteTable.Routes);
            
            // Filters
            
            // Factories
            var factory = new DependencyContainerControllerFactory(_dependencyContainer);
            ControllerBuilder.Current.SetControllerFactory(factory);           

            // Self-registration
            _dependencyContainer.RegisterInstance(_dependencyContainer);
            HttpContext.Current.Application[Globals.DependencyContainer] = _dependencyContainer;
        }

        private void RegisterCoreServices()
        {
            var core = Assembly.GetExecutingAssembly();
            var interfaces = core.GetInterfaces();
            
            RegisterAll(interfaces, core);
            RegisterAll(interfaces, _app);
        }

        private void RegisterAll(IEnumerable<Type> contracts, Assembly assembly)
        {
            foreach (var contract in contracts)
            {
                var implementations = assembly.GetConcreteTypesFor(contract);
                foreach(var implementation in implementations)
                {
                    _dependencyContainer.RegisterType(contract, implementation, implementation.Name);
                }
            }
        }
    }
}