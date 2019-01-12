using System.Web.Mvc;
using WorkOfFiction.Models;
using WorkOfFiction.Services;

namespace WorkOfFiction.Controllers
{
    public class CompositionController : Controller
    {
        private readonly CompositionService _compositionService;

        public CompositionController(CompositionService compositionService)
        {
            _compositionService = compositionService;
        }

        public ActionResult Index()
        {
            var compositions = _compositionService.GetAllCompositions();

            return View(compositions);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Composition composition)
        {

            if (ModelState.IsValid)
            {
                _compositionService.Insert(composition);

                return RedirectToAction("Index");
            }

            return View(composition);
        }
    }
}