using LibraryApplication.Models;
using LibraryApplication.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using NToastNotify;

namespace LibraryApplication.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;
        //private readonly IToastNotification _toastNotification;

        //public CategorysController(AppDbContext context, IToastNotification toastNotification)
        public CategoriesController(AppDbContext context)
        {
            _context = context;
            //_toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var categorys = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
            return View(categorys);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new CategoryViewModel();
            return View("CategoryForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CategoryForm", model);
            }

            var category = new Category
            {
                Name = model.Name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            //_toastNotification.AddSuccessToastMessage("Category created successfully");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            var viewModel = new CategoryViewModel
            {
                Name = category.Name
            };

            return View("CategoryForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CategoryForm", model);
            }

            var category = await _context.Categories.FindAsync(model.Id);

            if (category == null)
                return NotFound();


            category.Name = model.Name;
            _context.SaveChanges();

            //_toastNotification.AddSuccessToastMessage("Category updated successfully");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var category = await _context.Categories    
                .Include(c => c.Books)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok();
        }
    }
}