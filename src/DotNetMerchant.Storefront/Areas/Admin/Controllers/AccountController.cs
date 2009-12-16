using System.Web.Mvc;

namespace DotNetMerchant.Storefront.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
    }
}