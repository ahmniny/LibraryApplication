using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryApplication.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        public int? MainCategoryId { get; set; }
        public MainCategory? MainCategory { get; set; }
        public virtual List<Book>? Books { get; set; }
    }
    
}
