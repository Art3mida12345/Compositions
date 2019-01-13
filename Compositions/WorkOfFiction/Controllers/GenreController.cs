using System.Web.Mvc;
using WorkOfFiction.Models;
using WorkOfFiction.Services;

namespace WorkOfFiction.Controllers
{
    public class GenreController : Controller
    {
        private readonly GenreService _genreService;

        public GenreController(GenreService genreService)
        {
            _genreService = genreService;
        }

        public ActionResult Index()
        {
            var genres = _genreService.GetAllGenres();

            return View(genres);
        }

        public ActionResult Edit(int? id)
        {
            var genre = id.HasValue ? _genreService.GetGenre(id.Value) : new Genre();

            return View(genre);
        }

        [HttpPost]
        public ActionResult Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                var alreadyExist = _genreService.CheckIfAlreadyExist(genre);
                if (!alreadyExist)
                {
                    _genreService.Update(genre);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Name", $"Genre with name {genre.Name} already exists");
            }

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Genre genre)
        {
            if (genre.Id.HasValue)
            {
                var wasDeleted = _genreService.Delete(genre.Id.Value);

                if (!wasDeleted)
                    return View("Message", model: $"Genre: {genre.Name} can not be deleted due to active relations.");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult Delete(int? id)
        {
            var genre = _genreService.GetGenre(id);

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
                var alreadyExist = _genreService.CheckIfAlreadyExist(genre);
                if (!alreadyExist)
                {
                    _genreService.Insert(genre);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Name", $"Genre with name {genre.Name} already exists");
            }

            return View(genre);
        }
    }
}