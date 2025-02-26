
using AutoNest.Models.Cars;
using AutoNest.Services.Cars;
using AutoNest.Services.Engines;
using AutoNest.Services.Parts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminCarController: Controller
    {
        private readonly ICarService _carSerivce;
        private readonly IPartService _partsService;
        private readonly IEngineService _engineService;

        public AdminCarController(ICarService carSerivce,IPartService partsService,IEngineService engineService)
        {
            _carSerivce = carSerivce;
            _partsService = partsService;
            _engineService = engineService;
        }

        public IActionResult Index()
        {
            var cars = _carSerivce.GetAll();

            return View(cars);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CarInputModel car = new CarInputModel();

            car.AllParts = _partsService.GetAll().ToList();
            car.AllEngines = _engineService.GetAll().ToList();
            return View(car);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarInputModel carModel)
        {
            if (!ModelState.IsValid)
            {
                carModel.AllParts = _partsService.GetAll().ToList();
                carModel.AllEngines = _engineService.GetAll().ToList();
                return View(carModel);
            }

            await _carSerivce.AddCarAsync(carModel);
            return RedirectToAction("Index");
        }
        public IActionResult Details(string carId)
        {
            var car = _carSerivce.GetCarById(carId);



            return View();
        } 

        public IActionResult Delete(string carId)
        {
            _carSerivce.DeleteCar(carId);
            return RedirectToAction("Index");
        }
    }
}
