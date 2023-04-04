using LibraryApplication.Models;
using LibraryApplication.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace LibraryApplication.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IToastNotification _toastNotification;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedCoverSize = 1048576;

        public BooksController(AppDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.OrderBy(m => m.Title).ToListAsync();
            return View(books);
        }
        public async Task<IActionResult> SearchBooks()
        {
            var books = await _context.Books.OrderByDescending(m => m.Title).ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new BookViewModel
            {
                Authors = await _context.Authors.OrderBy(m => m.Name).ToListAsync(),
                MainCategories = _context.MainCategories.ToList(),
                Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync()
            };

            return View("BookForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Authors = await _context.Authors.OrderBy(m => m.Name).ToListAsync();
                model.MainCategories = _context.MainCategories;
                model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                return View("BookForm", model);
            }

            var files = Request.Form.Files;

            if (!files.Any())
            {
                model.Authors = await _context.Authors.OrderBy(m => m.Name).ToListAsync();
                model.MainCategories = _context.MainCategories;
                model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Cover", "Please select book cover!");
                return View("BookForm", model);
            }

            var cover = files.FirstOrDefault();

            if (!_allowedExtenstions.Contains(Path.GetExtension(cover.FileName).ToLower()))
            {
                model.Authors = await _context.Authors.OrderBy(m => m.Name).ToListAsync();
                model.MainCategories = _context.MainCategories;
                model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Cover", "Only .PNG, .JPG images are allowed!");
                return View("BookForm", model);
            }

            if (cover.Length > _maxAllowedCoverSize)
            {
                model.Authors = await _context.Authors.OrderBy(m => m.Name).ToListAsync();
                model.MainCategories = _context.MainCategories;
                model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Cover", "Cover cannot be more than 1 MB!");
                return View("BookForm", model);
            }

            using var dataStream = new MemoryStream();

            await cover.CopyToAsync(dataStream);

            var book = new Book
            {
                Title = model.Title,
                AuthorId = model.AuthorId,
                CategoryId = model.CategoryId,
                Year = model.Year,
                Description = model.Description,
                Cover = dataStream.ToArray()
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Book created successfully");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = await _context.Books.Where(b=>b.Id== id).Include(b => b.Category).FirstOrDefaultAsync();

            if (book == null)
                return NotFound();

            var viewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,

                MainCategoryId=  book.Category.MainCategoryId,
                CategoryId = book.CategoryId,
                Year = book.Year,
                Description = book.Description,
                Cover = book.Cover,
                Authors = await _context.Authors.OrderBy(m => m.Name).ToListAsync(),
                MainCategories = _context.MainCategories,
                Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync()
            };

            return View("BookForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Authors = await _context.Authors.OrderBy(m => m.Name).ToListAsync();
                model.MainCategories = _context.MainCategories;
                model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                return View("BookForm", model);
            }

            var book = await _context.Books.FindAsync(model.Id);

            if (book == null)
                return NotFound();

            var files = Request.Form.Files;

            if (files.Any())
            {
                var cover = files.FirstOrDefault();

                using var dataStream = new MemoryStream();

                await cover.CopyToAsync(dataStream);

                model.Cover = dataStream.ToArray();

                if (!_allowedExtenstions.Contains(Path.GetExtension(cover.FileName).ToLower()))
                {
                    model.Authors = await _context.Authors.OrderBy(m => m.Name).ToListAsync();
                    model.MainCategories = _context.MainCategories;
                    model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Cover", "Only .PNG, .JPG images are allowed!");
                    return View("BookForm", model);
                }

                if (cover.Length > _maxAllowedCoverSize)
                {
                    model.Authors = await _context.Authors.OrderBy(m => m.Name).ToListAsync();
                    model.MainCategories = _context.MainCategories;
                    model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Cover", "Cover cannot be more than 1 MB!");
                    return View("BookForm", model);
                }

                book.Cover = model.Cover;
            }

            book.Title = model.Title;
            book.AuthorId = model.AuthorId;
            book.CategoryId = model.CategoryId;
            book.Year = model.Year;
            book.Description = model.Description;

            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Book updated successfully");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = await _context.Books
            .Include(c => c.Category)
            .Include(a => a.Author)
            .SingleOrDefaultAsync(m => m.Id == id);

            if (book == null)
                return NotFound();

            return View(book);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            _context.SaveChanges();

            return Ok();
        
        }

        public IActionResult GetCategories(int mainCategoryId)
        {
            var categories=_context.Categories
                .Where(c=>c.MainCategoryId==mainCategoryId)
                .OrderBy(c=>c.Name).ToList();
            return Ok(categories);
        }
    }
}