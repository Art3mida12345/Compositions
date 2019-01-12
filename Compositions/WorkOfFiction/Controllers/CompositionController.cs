using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WorkOfFiction.Models;
using WorkOfFiction.Services;
using WorkOfFiction.ViewModels;

namespace WorkOfFiction.Controllers
{
    public class CompositionController : Controller
    {
        private readonly CompositionService _compositionService;
        private readonly AuthorService _authorService;
        private readonly TypeService _typeService;
        private readonly GenreService _genreService;
        private readonly LanguageService _languageService;

        public CompositionController(
            CompositionService compositionService,
            AuthorService authorService,
            TypeService typeService,
            GenreService genreService,
            LanguageService languageService)
        {
            _compositionService = compositionService;
            _authorService = authorService;
            _typeService = typeService;
            _genreService = genreService;
            _languageService = languageService;
        }

        public ActionResult Index()
        {
            var compositions = _compositionService.GetAllCompositions();

            return View(compositions);
        }

        public ViewResult Create()
        {
            ViewBag.Types = new SelectList(_typeService.GetAllTypes(), "Id", "Name");
            ViewBag.Genres = new MultiSelectList(_genreService.GetAllGenres(), "Id", "Name");
            ViewBag.Languages = new SelectList(_languageService.GetAllLanguages(), "Id", "Description");
            var authors = _authorService.GetAllAuthors().Select(author => new AuthorViewModel
            {
                Id = author.Id,
                FullName = author.Id + "-" + author.FirstName + " " + author.LastName
            });
            ViewBag.Authors = new SelectList(authors, "Id", "FullName");

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