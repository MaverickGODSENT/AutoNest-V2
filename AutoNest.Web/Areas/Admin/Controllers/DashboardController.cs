using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController:Controller
    {
        public DashboardController()
        {

        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
