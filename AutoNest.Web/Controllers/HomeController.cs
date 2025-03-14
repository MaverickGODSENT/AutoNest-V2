using AutoNest.Models.Home;
using AutoNest.Services.Cars;
using AutoNest.Services.Carts;
using AutoNest.Services.Categories;
using AutoNest.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AutoNest.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;
        private readonly ICartService _cartService;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ICarService carService, ICategoryService categoryService,ICartService cartService,UserManager<IdentityUser> userManager)
        {
            _carService = carService;
            _categoryService = categoryService;
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var cars = _carService.GetAll();
            var categories = _categoryService.GetAll();
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Cars = cars.ToList();
            homeViewModel.Categories = categories.ToList();
            await _cartService.InitCartForUser(userId);

            return View(homeViewModel);
        }

        public IActionResult Search(HomeViewModel homeViewModel)
        {
            var carId = homeViewModel.SelectedCarId;

            var categories = _categoryService.GetAll().ToList();
            foreach (var item in categories)
            {
                item.SelectedCarId = carId;
            }

            return View(categories);
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
