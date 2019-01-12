using System.Web.Mvc;
using WorkOfFiction.Services;

namespace WorkOfFiction.Controllers
{
    public class CountryController : Controller
    {
        private readonly CountryService _countryService;

        public CountryController(CountryService countryService)
        {
            _countryService = countryService;
        }

        public ActionResult Index()
        {
            var countries = _countryService.GetAllCountries();

            return View(countries);
        }
    }
}