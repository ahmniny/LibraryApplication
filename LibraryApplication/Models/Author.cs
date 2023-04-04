using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApplication.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Author
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Bio { get; set; }

        public List<Book> Books { get; set; }
    }
}
