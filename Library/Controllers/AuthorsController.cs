using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace Library.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public IActionResult Index()
        {
            return View(_authorService.GetAllAuthors());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var author = _authorService.GetAuthorDetails(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                author.Id = Guid.NewGuid();
                _authorService.AddAuthor(author);
                return RedirectToAction(nameof(Index));
            }

            return View(author);
        }
        public IActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var author = _authorService.GetAuthorDetails(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _authorService.UpdateAuthor(author);
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        //public IActionResult Delete(Guid id)
        //{
        //    if (id == Guid.Empty)
        //    {
        //        return NotFound();
        //    }

        //    var author = _authorService.GetAuthorDetails(id);
        //    if (author == null)
        //    {
        //        return NotFound();
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Json(new { success = false, error = "Invalid author ID." });
            }

            var author = _authorService.GetAuthorDetails(id);
            if (author == null)
            {
                return Json(new { success = false, error = "Author not found." });
            }

            try
            {
                _authorService.DeleteAuthor(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


    }
}
