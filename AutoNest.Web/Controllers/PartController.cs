using AutoNest.Services.Parts;
using Microsoft.AspNetCore.Mvc;

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
    }
}