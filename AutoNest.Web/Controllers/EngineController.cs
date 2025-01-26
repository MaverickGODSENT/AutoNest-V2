using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Controllers
{
    public class EngineController:Controller
    {
        public EngineController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
