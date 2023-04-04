using LibraryApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> SearchBooks(string searchQuery)
        {
            //var searchParam = new SqlParameter("@searchQuery", searchQuery);
            //var books =await _context.Database.SqlQueryRaw<Book>(
            //    "EXEC SearchBooksByAuthorOrTitle @searchQuery", searchParam).ToListAsync();
            //var books=_context.SearchBooksByAuthorOrTitle(searchQuery).ToList();

            var books =await  _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.Title.Contains(searchQuery) || b.Author.Name.Contains(searchQuery))
                //.Where(b => b.Title.Contains(searchQuery))
                .ToListAsync();
            return View(books);

        }


    }
}
