using AutoNest.Models.Home;
using AutoNest.Services.Cars;
using AutoNest.Services.Carts;
using AutoNest.Services.Categories;
using AutoNest.Services.Contacts;
using AutoNest.Services.Parts;
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
        private readonly IContactService _contactService;
        private readonly IPartService _partService;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ICarService carService, ICategoryService categoryService, ICartService cartService, IContactService contactService, IPartService partService, UserManager<IdentityUser> userManager)
        {
            _carService = carService;
            _categoryService = categoryService;
            _cartService = cartService;
            _contactService = contactService;
            _partService = partService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                await _cartService.InitCartForUser(userId);
            }
            var cars = _carService.GetAll();
            var categories = _categoryService.GetAll();
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Cars = cars.ToList();
            homeViewModel.Categories = categories.ToList();

            return View(homeViewModel);
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormModel contactFormModel)
        {
            await _contactService.Add(contactFormModel);

            return RedirectToAction("Index");
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

        [HttpPost]
        public IActionResult SearchBarSearch(string BrandModel)
        {
            var brand = BrandModel.Split(' ')[0];
            var model = BrandModel.Split(' ')[1];
            var part = _partService.GetAll().Where(p => p.Brand == brand && p.Model == model).ToList();
            if (part.Count() > 0)
            {
                return View("../Part/SearchResult", part);
            }
            else
            {
                return RedirectToAction("Index");
            }
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
