using System;
using Shofyi.Models;
using Shofyi.ViewModels.Categories;

namespace Shofyi.Services.Interface
{
	public interface ICategoryService
	{
        Task<List<Category>> GetAllAsync();
        Task CreateAsync(Category category);
        Task<List<CategoryVM>> GetAllOrderByDescAsync();
        Task<Category> GetWithProductAsync(int id);
    }
}

