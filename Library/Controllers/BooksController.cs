using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Interface;
using System.Security.Claims;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IAuthorService _authorService;
        private readonly IPublisherService _publisherService;

        public BooksController(IProductService productService, IShoppingCartService shoppingCartService, IAuthorService authorService, IPublisherService publisherService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _authorService = authorService;
            _publisherService = publisherService;
        }
        public IActionResult Index()
        {
            var books = _productService.GetAllBooks().ToList();
            return View(books);
        }


        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetBookDetails(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_authorService.GetAllAuthors(), "Id", "FirstName");
            ViewData["PublisherId"] = new SelectList(_publisherService.GetAllPublishers(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {

            if (ModelState.IsValid)
            {
                book.Id = Guid.NewGuid();
                _productService.AddBook(book);
                book.Author = _authorService.GetAuthorDetails(book.AuthorId);
                book.Publisher = _publisherService.GetPublisherDetails(book.PublisherId);
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if validation fails
            ViewData["AuthorId"] = new SelectList(_authorService.GetAllAuthors(), "Id", "FirstName", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_publisherService.GetAllPublishers(), "Id", "Name", book.PublisherId);

            return View(book);
        }

        [HttpPost]
        public IActionResult AddToCart(Guid bookId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookInShoppingCart = new BookInShoppingCart
            {
                BookId = bookId,
                Quantity = quantity,
            };

            var result = _shoppingCartService.AddBookToShoppingCart(bookInShoppingCart, userId);

            if (result)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, error = "Failed to add the book to cart." });
        }

        [HttpPost]
        public IActionResult AddToCartConfirmed(BookInShoppingCart model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.AddBookToShoppingCart(model, userId);

            return View("Index", _productService.GetAllBooks().ToList());
        }

        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetBookDetails(id);

            ViewData["AuthorId"] = new SelectList(_authorService.GetAllAuthors(), "Id", "FirstName", product.AuthorId);
            ViewData["PublisherId"] = new SelectList(_publisherService.GetAllPublishers(), "Id", "Name", product.PublisherId);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id,Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            book.Author = _authorService.GetAuthorDetails(book.AuthorId);
            book.Publisher = _publisherService.GetPublisherDetails(book.PublisherId);

            _productService.UpdateBook(book);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Json(new { success = false, error = "Invalid book ID." });
            }

            var book = _productService.GetBookDetails(id);
            if (book == null)
            {
                return Json(new { success = false, error = "Book not found." });
            }

            try
            {
                _productService.DeleteBook(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
