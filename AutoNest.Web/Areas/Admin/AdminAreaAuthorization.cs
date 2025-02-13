using Microsoft.AspNetCore.Mvc;

namespace AutoNest.Web.Areas.Admin
{
    public class AdminAreaAuthorization:Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
    }
}
