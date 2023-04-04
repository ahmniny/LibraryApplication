using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LibraryApplication.Models;

namespace LibraryApplication.ViewModel

{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public virtual List<Book>? Books { get; set; }

    }
}
