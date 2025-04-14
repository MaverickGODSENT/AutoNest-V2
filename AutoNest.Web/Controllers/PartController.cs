using AutoNest.Services.Parts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AutoNest.Web.Controllers
{
    public class PartController : Controller
    {
        private readonly IPartService _partService;

        public PartController(IPartService partService)
        {
            _partService = partService;
        }

        public IActionResult Index()
        {
            var parts = _partService.GetAll();
            return View(parts);
        }
        [HttpGet]
        public IActionResult Details(string id)
        {
            var part = _partService.GetAll().Where(p => p.Id == id).FirstOrDefault();
            return View(part);
        }

        public IActionResult SearchResult(string id, string selectedCarId)
        {
            var parts = _partService.GetPartsForCar(selectedCarId).Where(p => p.CategoryId == id).ToList();
            return View(parts);
        }
        public IActionResult Search(string searchTerm, string category, string sortBy, bool inStockOnly,string categoryId,string selectedCarId)
        {
            // Извличаме всички части
            var parts = _partService.GetPartsForCar(selectedCarId).Where(p => p.CategoryId == categoryId);

            // Филтрираме по търсене
            if (!string.IsNullOrEmpty(searchTerm))
            {
                parts = parts.Where(p => p.Model.Contains(searchTerm) || p.Description.Contains(searchTerm));
            }

            // Филтрираме по категория
            if (!string.IsNullOrEmpty(category))
            {
                parts = parts.Where(p => p.CategoryName == category);
            }

            // Филтрираме по наличност
            if (inStockOnly)
            {
                parts = parts.Where(p => p.InStock);
            }

            // Сортираме по цена
            switch (sortBy)
            {
                case "price-asc":
                    parts = parts.OrderBy(p => p.Price);
                    break;
                case "price-desc":
                    parts = parts.OrderByDescending(p => p.Price);
                    break;
                default:
                    parts = parts.OrderBy(p => p.Model); // Сортиране по модел по подразбиране
                    break;
            }

            // Връщаме филтрираните и сортирани части в изгледа
            return View(parts.ToList());
        }
    }
}
