using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shofyi.Data;
using Shofyi.Helpers;
using Shofyi.Models;
using Shofyi.Services.Interface;
using Shofyi.ViewModels.Categories;

namespace Shofyi.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context,
                                  ICategoryService categoryService,
                                  IWebHostEnvironment env)
        {
            _context = context;
            _categoryService = categoryService;
            _env = env;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _categoryService.GetAllOrderByDescAsync();
            return View(datas);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM request)
        {
            if (!ModelState.IsValid) return View();



            if (!request.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "File must be only image format");
                return View();
            };

            if (!request.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be max 200kb");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;

            string path = Path.Combine(_env.WebRootPath, "admin/assets/images", fileName);

            await request.Image.SaveFileToLocalAsync(path);

            await _categoryService.CreateAsync(new Category { Image = fileName, Name = request.Name });   

            return RedirectToAction("Index");

        }



        [HttpGet]
        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id is null) return BadRequest();
            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category is null) return NotFound();

            return View(new CategoryEditVM { Image = category.Image , Name=category.Name });
        }

            
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM request)
        {
            if (id is null) return BadRequest();
            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category is null) return NotFound();
            if (request.NewImage is null) return RedirectToAction(nameof(Index));

            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "File must be only image format");
                request.Image = category.Image;
                return View(request);
            };

            if (!request.NewImage.CheckFileSize(200))
            {
                ModelState.AddModelError("NewImage", "Image size must be max 200kb");
                request.Image = category.Image;
                return View(request);
            }

            string oldPath = Path.Combine(_env.WebRootPath, "img", category.Image);

            oldPath.DeleteFileFromLocal();

            string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;

            string newPath = Path.Combine(_env.WebRootPath, "admin/assets/images", fileName);

            await request.NewImage.SaveFileToLocalAsync(newPath);

            category.Image = fileName;
            category.Name = request.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Category category = await _context.Categories.Where(m => m.Id == id)
                                                  .FirstOrDefaultAsync();

            if (category is null) return NotFound();

            CategoryDetailVM model = new()
            {
                Name = category.Name,
                Image = category.Image,
            };

            return View(model);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category is null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "admin/assets/images", category.Image);
            path.DeleteFileFromLocal();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
