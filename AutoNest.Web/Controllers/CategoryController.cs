using AutoNest.Models.Categories;
using AutoNest.Services.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var categories = _categoryService.GetAll();
            return View(categories);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryAddViewModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Index");
            }

            await _categoryService.AddCategoryAsync(categoryModel);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditCategory(string id)
        {
            var category = _categoryService.GetAll().Where(c => c.Id == id).FirstOrDefault();
            CategoryViewModel categoryViewModel = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return View(category);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoryViewModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            await _categoryService.UpdateCategoryAsync(categoryModel);
            return RedirectToAction("Index");
        }
    }
}
