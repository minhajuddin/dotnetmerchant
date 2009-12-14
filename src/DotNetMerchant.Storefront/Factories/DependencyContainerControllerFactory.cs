using System;
using System.Web.Mvc;
using System.Web.Routing;
using DotNetMerchant.Storefront.Configuration;

namespace DotNetMerchant.Storefront.Factories
{
    public class DependencyContainerControllerFactory : DefaultControllerFactory
    {
        private readonly IDependencyContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContainerControllerFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public DependencyContainerControllerFactory(IDependencyContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if(controllerType == null)
            {
                return null;
            }

            return (IController)_container.Resolve(controllerType);
        }
    }
}