using System.Web.Mvc;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;

namespace WorkOfFiction.Controllers
{
    //TODO: REMINDER: add constraint uniq to all fields such names and short code
    public class LanguageController : Controller
    {
        private readonly OracleHelper _oracleHelper;

        public LanguageController()
        {
            _oracleHelper = new OracleHelper();
        }

        // GET: Genre
        public ActionResult Index()
        {
            var languages = _oracleHelper.GetAllLanguages();

            return View(languages);
        }

        public ActionResult Edit(int? id)
        {
            var language = id.HasValue ? _oracleHelper.GetLanguage(id.Value) : new Language();

            return View(language);
        }

        [HttpPost]
        public ActionResult Edit(Language language)
        {
            if (ModelState.IsValid)
            {
                _oracleHelper.Update(TableName.Languages, language.ToStringExtension());

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
                _oracleHelper.Delete(TableName.Languages, language.Id.Value);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult Delete(int? id)
        {
            var language = _oracleHelper.GetLanguage(id);

            if (language != null)
            {
                return PartialView(language);
            }

            return PartialView("Message", model: "Language not found");
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
                _oracleHelper.Insert(TableName.Languages, language.ToStringExtension(false));

                return RedirectToAction("Index");
            }

            return View(language);
        }
    }
}