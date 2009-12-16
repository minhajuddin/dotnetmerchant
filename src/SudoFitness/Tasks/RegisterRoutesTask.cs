using System.Web.Mvc;
using System.Web.Routing;
using DotNetMerchant.Storefront.Configuration;

namespace SudoFitness.Tasks
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
            _routes.MapRoute(
                "Default",                                               // Route name
                "{controller}/{action}/{id}",                            // URL with parameters
                new { controller = "Home", action = "Index", id = "" },  // Parameter defaults
                new[] { "SudoFitness.Controllers" }                      // Namespaces
                );
        }
    }
}