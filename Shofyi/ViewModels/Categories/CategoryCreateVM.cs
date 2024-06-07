using System;
using System.ComponentModel.DataAnnotations;

namespace Shofyi.ViewModels.Categories
{
	public class CategoryCreateVM
	{
        [Required(ErrorMessage = "This input can't be empty")]
        [StringLength(20, ErrorMessage = "Length must be max 20")]
        public string? Name { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}

