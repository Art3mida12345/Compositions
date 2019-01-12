using System.Web.Mvc;
using WorkOfFiction.Helpers;

namespace WorkOfFiction.Controllers
{
    public class CountryController : Controller
    {
        private readonly Count _oracleHelper;

        public CountryController()
        {
            _oracleHelper = new OracleHelper();
        }

        // GET: Country
        public ActionResult Index()
        {
            return View();
        }
    }
}