using System.Web.Mvc;

namespace WorkOfFiction.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}