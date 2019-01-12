using System.Web.Mvc;
using WorkOfFiction.Models;
using WorkOfFiction.Services;

namespace WorkOfFiction.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AuthorService _authorService;
        private readonly CountryService _countryService;


        public AuthorController(AuthorService authorService, CountryService countryService)
        {
            _authorService = authorService;
            _countryService = countryService;
        }

        public ActionResult Index()
        {
            var authors = _authorService.GetAllAuthors();

            return View(authors);
        }

        public ActionResult Edit(int? id)
        {
            var countries = _countryService.GetAllCountries();

            ViewBag.Countries = new SelectList(countries, "Id", "CountryName");

            var author = id.HasValue ? _authorService.GetAuthor(id.Value) : new Author();

            return View(author);
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                var author = _authorService.GetAuthor(id.Value);
                return View(author);
            }

            return View("Message", model: "Author id cannot be null");
        }


        [HttpPost]
        public ActionResult Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                _authorService.Update(author);
                return RedirectToAction("Index");
            }

            return View(author);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Author author)
        {
            if (author.Id.HasValue)
            {
                var wasDeleted = _authorService.Delete(author.Id.Value);

                if (!wasDeleted)
                {
                    return View("Message", model: $"Author: {author.FirstName + " " + author.LastName} has active relations. You can not delete this author");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult Delete(int? id)
        {
            var author = _authorService.GetAuthor(id);

            if (author != null)
            {
                return PartialView(author);
            }

            return PartialView("Message", model: "Author not found");
        }

        public ViewResult Create()
        {
            var countries = _countryService.GetAllCountries();

            ViewBag.Countries = new SelectList(countries, "Id", "CountryName");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Author author)
        {


            if (ModelState.IsValid)
            {
                _authorService.Insert(author);
                return RedirectToAction("Index");
            }

            return View(author);
        }
    }
}