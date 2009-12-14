using System.Web;
using DotNetMerchant.Storefront;
using DotNetMerchant.Storefront.Configuration;

namespace SudoFitness
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Bootstrapper<StorefrontConfiguration>.Run();
        }
    }
}