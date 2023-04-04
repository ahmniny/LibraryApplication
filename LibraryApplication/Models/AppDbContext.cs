using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LibraryApplication.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MainCategory> MainCategories { get; set; }

        public List<Book> SearchBooksByAuthorOrTitle(string searchQuery)
        {

            var searchParam = new SqlParameter("@searchQuery", searchQuery);
            //var books = this.Books
            //    .FromSqlRaw("EXEC SearchBooksByAuthorOrTitle @searchQuery", searchParam)
            //    .Include(b => b.Author).Include(b => b.Category)
            //    .ToList();
            //var books = this.Books.Where((b=>b.Title.Contains(searchQuery) || b.Author.Name.Contains(searchQuery)))
            //                .FromSqlRaw("EXEC SearchBooksByAuthorOrTitle @searchQuery", searchParam)
            //                .Include(b => b.Author).Include(b => b.Category)
            //                .ToList();
            var books = Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                //.Where(b => b.Title.Contains(searchQuery) || b.Author.Name.Contains(searchQuery))
                .Where(b => b.Title.Contains(searchQuery))
                .ToList();

            //var searchParam = new SqlParameter("@searchQuery", searchQuery);
            //var books = this.Books.SqlQueryRaw<Book>(
            //    "EXEC SearchBooksByAuthorOrTitle @searchQuery", searchParam).ToList();

            return books;
        }
    }
}
