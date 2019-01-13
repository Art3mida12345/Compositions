using System.Web.Mvc;
using WorkOfFiction.Models;
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

        public ActionResult Edit(int? id)
        {
            var country = id.HasValue ? _countryService.GetCountry(id.Value) : new Country();

            return View(country);
        }

        [HttpPost]
        public ActionResult Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                _countryService.Update(country);

                return RedirectToAction("Index");
            }

            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Genre genre)
        {
            if (genre.Id.HasValue)
            {
                var wasDeleted = _countryService.Delete(genre.Id.Value);

                if (!wasDeleted)
                    return View("Message", model: $"Country: {genre.Name} can not be deleted due to active relations.");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult Delete(int? id)
        {
            var country = _countryService.GetCountry(id);

            if (country != null)
            {
                return PartialView(country);
            }

            return PartialView("Message", model: "Country not found");
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Country country)
        {

            if (ModelState.IsValid)
            {
                _countryService.Insert(country);

                return RedirectToAction("Index");
            }

            return View(country);
        }
    }
}