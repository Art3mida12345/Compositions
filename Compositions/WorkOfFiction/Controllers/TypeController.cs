using System.Web.Mvc;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;
using WorkOfFiction.Services;

namespace WorkOfFiction.Controllers
{
    public class TypeController : Controller
    {
        private readonly TypeService _typeService;

        public TypeController(TypeService typeService)
        {
            _typeService = typeService;
        }

        // GET: Type
        public ActionResult Index()
        {
            var types = _typeService.GetAllTypes();

            return View(types);
        }

        public ActionResult Edit(int? id)
        {
            var type = id.HasValue ? _typeService.GetType(id.Value) : new Type();

            return View(type);
        }

        [HttpPost]
        public ActionResult Edit(Type type)
        {
            if (ModelState.IsValid)
            {
                var alreadyEXist = _typeService.CheckIfAlreadyExist(type);
                if (!alreadyEXist)
                {
                    _typeService.Update(type);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Name", $"Type with name {type.Name} already exists");                

            }

            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult Delete(Type type)
        {
            if (type.Id.HasValue)
            {
                _typeService.Delete(type.Id.Value);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult Delete(int? id)
        {
            var type = _typeService.GetType(id);

            if (type != null)
            {
                return PartialView(type);
            }

            return PartialView("Message", model: "Type not found");
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Type type)
        {
            if (ModelState.IsValid)
            {
                var alreadyEXist = _typeService.CheckIfAlreadyExist(type);
                if (!alreadyEXist)
                {
                    _typeService.Insert(type);

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Name", $"Type with name {type.Name} already exists");                

            }

            return View(type);
        }
    }
}