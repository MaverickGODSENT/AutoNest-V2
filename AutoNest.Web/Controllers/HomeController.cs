using System.Diagnostics;
using AutoNest.Models.Home;
using AutoNest.Models.Parts;
using AutoNest.Services.Cars;
using AutoNest.Services.Categories;
using AutoNest.Services.Parts;
using AutoNest.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPartService _partService;
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;

        public HomeController(IPartService partService, ICarService carService,ICategoryService categoryService)
        {
            _partService = partService;
            _carService = carService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var cars = _carService.GetAll();
            var categories = _categoryService.GetAll();
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Cars = cars.ToList();
            homeViewModel.Categories = categories.ToList();

            return View(homeViewModel);
        }

        public IActionResult Search(HomeViewModel homeViewModel)
        {
            var carId = homeViewModel.SelectedCarId;

            var categories = _categoryService.GetAll().ToList();
            foreach(var item in categories)
            {
                item.SelectedCarId = carId;
            }

            return View(categories);
        }



        public IActionResult GetFilteredParts(string searchString, string categoryFilter, string sortOrder)
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
