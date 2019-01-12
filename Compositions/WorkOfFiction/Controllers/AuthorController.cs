using System.Web.Mvc;
using WorkOfFiction.Models;
using WorkOfFiction.Services;

namespace WorkOfFiction.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AuthorService _authorService;


        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        public ActionResult Index()
        {
            var authors = _authorService.GetAllAuthors();

            return View(authors);
        }

        public ActionResult Edit(int? id)
        {
            var author = id.HasValue ? _authorService.GetAuthor(id.Value) : new Author();

            return View(author);
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
        public RedirectToRouteResult Delete(Author author)
        {
            if (author.Id.HasValue)
            {
                _authorService.Delete(author.Id.Value);
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