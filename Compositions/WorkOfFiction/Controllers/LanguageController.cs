using System.Web.Mvc;
using WorkOfFiction.Models;
using WorkOfFiction.Services;

namespace WorkOfFiction.Controllers
{
    public class LanguageController : Controller
    {
        private readonly LanguageService _languageService;

        public LanguageController(LanguageService languageService)
        {
            _languageService = languageService;
        }

        // GET: Genre
        public ActionResult Index()
        {
            var languages = _languageService.GetAllLanguages();

            return View(languages);
        }

        public ActionResult Edit(int? id)
        {
            var language = id.HasValue ? _languageService.GetLanguage(id.Value) : new Language();

            return View(language);
        }

        [HttpPost]
        public ActionResult Edit(Language language)
        {
            if (ModelState.IsValid)
            {
                _languageService.Update(language);

                return RedirectToAction("Index");
            }

            return View(language);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult Delete(Language language)
        {
            if (language.Id.HasValue)
            {
                _languageService.Delete(language.Id.Value);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var language = _languageService.GetLanguage(id);

            if (language != null)
            {
                return PartialView(language);
            }

            return View("Message", model: "Language not found");
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Language language)
        {
            if (ModelState.IsValid)
            {
                var alreadyExist = _languageService.CheckIfAlreadyExist(language);
                if (!alreadyExist)
                {
                    _languageService.Insert(language);

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Name", $"Genre with name {language.Description} already exists");
            }

            return View(language);
        }
    }
}