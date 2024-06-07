using System;
using Microsoft.AspNetCore.Mvc;
using Shofyi.Data;
using Shofyi.Models;
using Shofyi.Services.Interface;
using Shofyi.ViewModels;

namespace Shofyi.Controllers
{
	public class HomeController : Controller
	{
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;

        public HomeController(AppDbContext context,
                              ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _categoryService.GetAllAsync();


            HomeVM model = new()
            {
                Categories = categories
            };

            return View(model);
        }
    }
}

