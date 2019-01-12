using System.Web.Mvc;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;

namespace WorkOfFiction.Controllers
{
    public class TypeController : Controller
    {
        private readonly OracleHelper _oracleHelper;

        public TypeController()
        {
            _oracleHelper = new OracleHelper();
        }

        // GET: Type
        public ActionResult Index()
        {
            var types = _oracleHelper.GetAllTypes();

            return View(types);
        }

        public ActionResult Edit(int? id)
        {
            var type = id.HasValue ? _oracleHelper.GetType(id.Value) : new Type();

            return View(type);
        }

        [HttpPost]
        public ActionResult Edit(Type type)
        {
            if (ModelState.IsValid)
            {
                _oracleHelper.Update(TableName.Types, type.Id, type.ToStringExtension());

                return RedirectToAction("Index");
            }

            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult Delete(Type type)
        {
            if (type.Id.HasValue)
            {
                _oracleHelper.Delete(TableName.Types, type.Id.Value);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult Delete(int? id)
        {
            var type = _oracleHelper.GetType(id);

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
                _oracleHelper.Insert(TableName.Types, type.ToStringExtension(false));

                return RedirectToAction("Index");
            }

            return View(type);
        }
    }
}