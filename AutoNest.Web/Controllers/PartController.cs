using AutoNest.Models.Parts;
using AutoNest.Services.Categories;
using AutoNest.Services.Parts;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Controllers
{
    public class PartController : Controller
    {
        private readonly IPartService _partService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        public PartController(IPartService partService, ICategoryService categoryService ,IWebHostEnvironment environment)
        {
            _partService = partService;
            _categoryService = categoryService;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var parts = _partService.GetAll();
            return View(parts);
        }
        [HttpGet]
        public IActionResult CreatePart()
        {
            PartAddViewModel partAddView = new PartAddViewModel();
            var categories = _categoryService.GetAll();
            List<KeyValuePair<string,string>> result = new List<KeyValuePair<string, string>>();
            foreach (var category in categories)
            {
                var KeyValuePair = new KeyValuePair<string, string>(category.Id, category.Name);
                result.Add(KeyValuePair);
            }
            partAddView.Categories = result;
            return View(partAddView);
        }
        [HttpPost]
        public IActionResult CreatePart(PartAddViewModel partAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Index");
            }
            _partService.Add(partAddViewModel, $"{_environment.WebRootPath}/images");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditPart(string id)
        {
            var part = _partService.GetAll().Where(p => p.Id == id).FirstOrDefault();

            return View(part);
        }
        [HttpPost]
        public IActionResult EditPart(PartViewModel partViewModel)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Index");
            }
            _partService.Update(partViewModel);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeletePart(string id)
        {
            _partService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
