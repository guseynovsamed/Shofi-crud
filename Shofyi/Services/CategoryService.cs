using System;
using Microsoft.EntityFrameworkCore;
using Shofyi.Data;
using Shofyi.Models;
using Shofyi.Services.Interface;
using Shofyi.ViewModels.Categories;

namespace Shofyi.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly AppDbContext _context;

        public CategoryService( AppDbContext context)
        {
            _context = context;
        }


        public async Task CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.Where(m => !m.SoftDeleted).ToListAsync();
        }


        public async Task<List<CategoryVM>> GetAllOrderByDescAsync()
        {
            var categories = await _context.Categories.OrderByDescending(m => m.Id).ToListAsync();
            return categories.Select(m => new CategoryVM { Id = m.Id, Name = m.Name , Image=m.Image }).ToList();
        }


        public async Task<Category> GetWithProductAsync(int id)
        {
            return await _context.Categories.Where(m => m.Id == id).FirstOrDefaultAsync();
        }


      

    }
}

