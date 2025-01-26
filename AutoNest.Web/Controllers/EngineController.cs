using AutoNest.Models.Categories;
using AutoNest.Models.Engines;
using AutoNest.Services.Engines;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Controllers
{
    public class EngineController:Controller
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
        [HttpGet]
        public IActionResult CreateEngine()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateEngine(EngineAddViewModel engineModel)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Index");
            }

            _engineService.Add(engineModel);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteCategory(string id)
        {
            _engineService.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditCategory(string id)
        {
            var engine = _engineService.GetAll().Where(e => e.Id == id).FirstOrDefault();
            new EngineViewModel
            {
                Id = engine.Id,
                EngineCode = engine.EngineCode,
                EngineDisplacement = engine.EngineDisplacement,
                EngineHorsePower = engine.EngineHorsePower,
                Transmission = engine.Transmission,
                Drivetrain = engine.Drivetrain,
            };
            return View(engine);
        }
        [HttpPost]
        public IActionResult EditCategory(EngineViewModel engineModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            _engineService.Update(engineModel);
            return RedirectToAction("Index");
        }
    }
}
