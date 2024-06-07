using System;
namespace Shofyi.ViewModels.Categories
{
	public class CategoryEditVM
	{
        public string? Name { get; set; }
        public string? Image { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}

