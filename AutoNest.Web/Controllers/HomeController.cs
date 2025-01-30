using System.Diagnostics;
using AutoNest.Models.Parts;
using AutoNest.Services.Parts;
using AutoNest.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPartService _partService;

        public HomeController(IPartService partService)
        {
            _partService = partService;
        }

        public IActionResult Index()
        {
            List<PartViewModel> parts = _partService.GetAll().ToList();





            return View(parts);
        }

        public async Task<IActionResult> GetFilteredParts(string searchString, string categoryFilter, string sortOrder)
        {
            var parts = _partService.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                parts = parts.Where(p => p.Brand.Contains(searchString, StringComparison.OrdinalIgnoreCase) || p.Model.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            if(!String.IsNullOrEmpty(categoryFilter))
            {
                parts = parts.Where(p => p.CategoryName == categoryFilter);
            }

            switch (sortOrder)
            {
                case "price_asc":
                    parts = parts.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    parts = parts.OrderByDescending(p => p.Price);
                    break;
                case "sales_asc":
                    parts = parts.OrderBy(p => p.SalesCount);
                    break;
                case "sales_desc":
                    parts = parts.OrderByDescending(p => p.SalesCount);
                    break;
                default:
                    parts = parts.OrderBy(p => p.Brand);
                    break;
            }

            return PartialView("_PartsPartial", parts.ToList());
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
