using System.Web.Mvc;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;

namespace WorkOfFiction.Controllers
{
    public class GenreController : Controller
    {
        private readonly OracleHelper _oracleHelper;

        public GenreController()
        {
            _oracleHelper = new OracleHelper();
        }

        public ActionResult Index()
        {
            var genres = _oracleHelper.GetAllGenres();

            return View(genres);
        }

        public ActionResult Edit(int? id)
        {
            var genre = id.HasValue ? _oracleHelper.GetGenre(id.Value) : new Genre();

            return View(genre);
        }

        [HttpPost]
        public ActionResult Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _oracleHelper.Update(TableName.Genres, genre.ToStringExtension());

                return RedirectToAction("Index");
            }

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult Delete(Genre genre)
        {
            if (genre.Id.HasValue)
            {
                _oracleHelper.Delete(TableName.Genres, genre.Id.Value);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult Delete(int? id)
        {
            var genre = _oracleHelper.GetGenre(id);

            if (genre != null)
            {
                return PartialView(genre);
            }

            return PartialView("Message", model: "Genre not found");
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _oracleHelper.Insert(TableName.Genres, genre.ToStringExtension(false));

                return RedirectToAction("Index");
            }

            return View(genre);
        }
    }
}