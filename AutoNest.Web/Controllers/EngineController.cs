using AutoNest.Models.Categories;
using AutoNest.Models.Engines;
using AutoNest.Services.Engines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Controllers
{
    public class EngineController : Controller
    {
        private readonly IEngineService _engineService;
        public EngineController(IEngineService engineService)
        {
            _engineService = engineService;
        }

        public IActionResult Index()
        {
            var engines = _engineService.GetAll();

            return View(engines);
        }
    }
}
