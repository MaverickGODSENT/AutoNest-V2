using AutoNest.Models.Categories;
using AutoNest.Services.Categories;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Controllers
{
    public class CategoryController:Controller
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
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryAddViewModel categoryModel)
        {
            if(!ModelState.IsValid)
            {
                RedirectToAction("Index");
            }

            _categoryService.Add(categoryModel);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteCategory(string id)
        {
            _categoryService.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditCategory(string id)
        {
            var category = _categoryService.GetAll().Where(c=>c.Id==id).FirstOrDefault();
            CategoryViewModel categoryViewModel = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return View(category);
        }
        [HttpPost]
        public IActionResult EditCategory(CategoryViewModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            _categoryService.Update(categoryModel);
            return RedirectToAction("Index");
        }
    }
}
