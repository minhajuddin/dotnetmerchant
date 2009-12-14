using System.Web.Mvc;
using System.Web.Routing;
using DotNetMerchant.Storefront.Configuration;

namespace DotNetMerchant.Storefront.Tasks
{
    public class RegisterRoutesTask : IBootstrapperTask
    {
        private readonly RouteCollection _routes;

        public RegisterRoutesTask()
            : this(RouteTable.Routes)
        {

        }

        public RegisterRoutesTask(RouteCollection routes)
        {
            _routes = routes;
        }

        public void Execute()
        {
            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            AreaRegistration.RegisterAllAreas();

            _routes.MapRoute(
                "Default",                                               // Route name
                "{controller}/{action}/{id}",                            // URL with parameters
                new { controller = "Home", action = "Index", id = "" },  // Parameter defaults
                new[] { "DotNetMerchant.Storefront.Controllers" }        // Namespaces
                );

            HasExecuted = true;
        }

        public bool HasExecuted { get; private set; }
    }
}
