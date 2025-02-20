
using AutoNest.Services.Cars;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Areas.Admin.Controllers
{
    public class AdminCarController: Controller
    {
        private readonly ICarService _carSerivce;

        public AdminCarController(ICarService carSerivce)
        {
            _carSerivce = carSerivce;
        }

        public IActionResult Index()
        {
            var cars = _carSerivce.GetAll();
            return View(cars);
        }
    }
}
