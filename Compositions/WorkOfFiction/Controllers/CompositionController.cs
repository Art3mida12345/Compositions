﻿using System.Linq;
using System.Web.Mvc;
using WorkOfFiction.Models;
using WorkOfFiction.Services;
using WorkOfFiction.ViewModels;
using CompositionFilter = WorkOfFiction.Models.CompositionFilter;

namespace WorkOfFiction.Controllers
{
    public class CompositionController : Controller
    {
        private readonly CompositionService _compositionService;
        private readonly AuthorService _authorService;
        private readonly TypeService _typeService;
        private readonly GenreService _genreService;
        private readonly LanguageService _languageService;
        private readonly FilterService _filterService;
        public CompositionController(
            CompositionService compositionService,
            AuthorService authorService,
            TypeService typeService,
            GenreService genreService,
            LanguageService languageService, 
            FilterService filterService)
        {
            _compositionService = compositionService;
            _authorService = authorService;
            _typeService = typeService;
            _genreService = genreService;
            _languageService = languageService;
            _filterService = filterService;
        }

        public ActionResult Index(CompositionFilter filter)
        {
            var filteredCompositions = _filterService.ApplyFilter(filter);

            InitializeFilterModel(filter);
            ViewBag.Filter = filter;

            return View(filteredCompositions);
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

            ViewBag.Types = new SelectList(_typeService.GetAllTypes(), "Id", "Name");
            ViewBag.Genres = new MultiSelectList(_genreService.GetAllGenres(), "Id", "Name");
            ViewBag.Languages = new SelectList(_languageService.GetAllLanguages(), "Id", "Description");
            var authors = _authorService.GetAllAuthors().Select(author => new AuthorViewModel
            {
                Id = author.Id,
                FullName = author.Id + "-" + author.FirstName + " " + author.LastName
            });
            ViewBag.Authors = new SelectList(authors, "Id", "FullName");

            return View(composition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Composition composition)
        {
            if (composition.Id.HasValue)
            {
                var result = _compositionService.Delete(composition.Id.Value);

                if (!result)
                {
                    return View("Message", model: $"Composition with id = {composition.Id} has relation and can not be delete.");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var composition = _compositionService.GetComposition(id);

            if (composition != null)
            {
                return PartialView(composition);
            }

            return View("Message", model: "Composition not found");
        }

        public ActionResult Details(int? id)
        {
            var composition = _compositionService.GetComposition(id);

            return View(composition);
        }

        public ActionResult Edit(int? id)
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

            var composition = id.HasValue ? _compositionService.Get(id) : new Composition();
            composition.AuthorsIds = composition.Authors.Select(a => a.Id).ToArray();
            composition.GenresIds = composition.Genres.Select(g => g.Id).ToArray();

            return View(composition);
        }

        [HttpPost]
        public ActionResult Edit(Composition composition)
        {
            if (ModelState.IsValid)
            {
                _compositionService.Edit(composition);

                return RedirectToAction("Index");
            }

            ViewBag.Types = new SelectList(_typeService.GetAllTypes(), "Id", "Name");
            ViewBag.Genres = new MultiSelectList(_genreService.GetAllGenres(), "Id", "Name");
            ViewBag.Languages = new SelectList(_languageService.GetAllLanguages(), "Id", "Description");
            var authors = _authorService.GetAllAuthors().Select(author => new AuthorViewModel
            {
                Id = author.Id,
                FullName = author.Id + "-" + author.FirstName + " " + author.LastName
            });
            ViewBag.Authors = new SelectList(authors, "Id", "FullName");

            return View(composition);
        }

        private void InitializeFilterModel(CompositionFilter filter)
        {
            if (filter == null)
            {
                filter = new CompositionFilter();
            }

            var genres = _genreService.GetAllGenres();
            var authors = _authorService.GetAllAuthors();
            var types = _typeService.GetAllTypes();
            var languages = _languageService.GetAllLanguages();

            foreach (var genre in genres)
            {
                filter.Genres.Add(
                    new CheckBoxViewModel
                    {
                        Id = genre.Id.Value,
                        Value = genre.Name,
                        IsChecked = filter.SelectedGenres.Contains(genre.Id.Value)
                    });
            }
            foreach (var author in authors)
            {
                filter.Authors.Add(
                    new CheckBoxViewModel
                    {
                        Id = author.Id.Value,
                        Value = author.FirstName + " " + author.LastName + " " + author.DateBirth.ToShortDateString(),
                        IsChecked = filter.SelectedAuthors.Contains(author.Id.Value)
                    });
            }
            foreach (var type in types)
            {
                filter.Types.Add(
                    new CheckBoxViewModel
                    {
                        Id = type.Id.Value,
                        Value = type.Name,
                        IsChecked = filter.SelectedTypes.Contains(type.Id.Value)
                    });
            }
            foreach (var lang in languages)
            {
                filter.Langs.Add(
                    new CheckBoxViewModel
                    {
                        Id = lang.Id.Value,
                        Value = lang.ShortCode,
                        IsChecked = filter.SelectedLangs.Contains(lang.Id.Value)
                    });
            }
        }
    }
}