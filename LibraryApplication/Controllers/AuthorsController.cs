using LibraryApplication.Models;
using LibraryApplication.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using NToastNotify;

namespace LibraryApplication.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly AppDbContext _context;
        //private readonly IToastNotification _toastNotification;

        //public AuthorsController(AppDbContext context, IToastNotification toastNotification)
        public AuthorsController(AppDbContext context)
        {
            _context = context;
            //_toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await _context.Authors.OrderBy(m => m.Name).ToListAsync();
            return View(authors);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new AuthorViewModel();
            return View("AuthorForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AuthorForm", model);
            }

            var author = new Author
            {
                Name = model.Name,
                Bio = model.Bio
            };

            _context.Authors.Add(author);
            _context.SaveChanges();

            //_toastNotification.AddSuccessToastMessage("Author created successfully");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
                return NotFound();

            var viewModel = new AuthorViewModel
            {
                Name = author.Name,
                Bio = author.Bio
            };

            return View("AuthorForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AuthorForm", model);
            }

            var author = await _context.Authors.FindAsync(model.Id);

            if (author == null)
                return NotFound();


            author.Name = model.Name;
            author.Bio = model.Bio;
            _context.SaveChanges();

            //_toastNotification.AddSuccessToastMessage("Author updated successfully");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var author = await _context.Authors
                .Include(c => c.Books)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (author == null)
                return NotFound();

            return View(author);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
                return NotFound();

            _context.Authors.Remove(author);
            _context.SaveChanges();

            return Ok();
        }
    }
}