using System.Web.Mvc;

namespace DotNetMerchant.Storefront.Areas.Admin
{
    public class Routes : AreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "dotnetmerchant_admin_login",
                "admin/login",
                new { controller = "Account", action = "Login", id = "" },
                new[] { "DotNetMerchant.Storefront.Areas.Admin.Controllers" }
                );

            context.MapRoute(
                "dotnetmerchant_admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" },
                new[] { "DotNetMerchant.Storefront.Areas.Admin.Controllers" }
                );
        }

        public override string AreaName
        {
            get { return "Admin"; }
        }
    }
}