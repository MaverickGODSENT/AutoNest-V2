using AutoNest.Models.Engines;
using AutoNest.Services.Engines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Areas.Admin.Controllers
{
    public class AdminEngineController : Controller
    {
        private readonly IEngineService _engineService;
        public AdminEngineController(IEngineService engineService)
        {
            _engineService = engineService;
        }

        public IActionResult Index()
        {
            var engines = _engineService.GetAll();

            return View(engines);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateEngine()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEngine(EngineAddViewModel engineModel)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Index");
            }

            await _engineService.AddEngineAsync(engineModel);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> DeleteEngine(string id)
        {
            await _engineService.DeleteEngineAsync(id);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditEngine(string id)
        {
            var engine = _engineService.GetAll().Where(e => e.Id == id).FirstOrDefault();
            var newEngine = new EngineViewModel
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditEngine(EngineViewModel engineModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            await _engineService.UpdateEngineAsync(engineModel);
            return RedirectToAction("Index");
        }
    }
}
