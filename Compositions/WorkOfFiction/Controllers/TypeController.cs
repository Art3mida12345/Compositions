using System.Web.Mvc;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;

namespace WorkOfFiction.Controllers
{
    public class TypeController : Controller
    {
        private OracleHelper _oracleHelper;

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

        public ActionResult Edit(string id)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Delete(string id)
        {
            throw new System.NotImplementedException();
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