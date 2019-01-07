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
                _oracleHelper.Update(TableName.Types, type.ToStringExtension());

                return RedirectToAction("Index");
            }

            return View(type);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                _oracleHelper.Delete(TableName.Types, id.Value);
            }

            return RedirectToAction("Index");
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
                _oracleHelper.Insert(TableName.Types, type.Name);

                return RedirectToAction("Index");
            }

            return View(type);
        }
    }
}