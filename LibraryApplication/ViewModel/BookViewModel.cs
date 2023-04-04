using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LibraryApplication.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApplication.ViewModel
{
    public class BookViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(250)]
        public string Title { get; set; }

        public int Year { get; set; }


        [Required, StringLength(2500)]
        public string Description { get; set; }

        [Display(Name = "Select book cover...")]
        public byte[]? Cover { get; set; }

        [Display(Name = "Main Category")]
        
        public int? MainCategoryId { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        public IEnumerable<Author>? Authors { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        
        public IEnumerable<MainCategory>? MainCategories { get; set; }

    }
}
