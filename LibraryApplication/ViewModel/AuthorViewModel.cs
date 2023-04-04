using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LibraryApplication.Models;

namespace LibraryApplication.ViewModel

{
    public class AuthorViewModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Bio { get; set; }

        public List<Book>? Books { get; set; }
    }
}
