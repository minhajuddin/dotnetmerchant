using System.Web.Mvc;

namespace DotNetMerchant.Storefront.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}